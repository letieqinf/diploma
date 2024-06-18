using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Office.Interop.Word;
using Newtonsoft.Json.Linq;
using NuGet.Packaging;
using System.Reflection;
using System.Text;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Ports.Input;
using Tusur.Practices.Persistence.Database.Entities;
using Tusur.Practices.Server.Extensions;
using Tusur.Practices.Server.Models.Service;

namespace Tusur.Practices.Server.Controllers
{
    [Route("api/documents")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DocumentController : ControllerBase
    {
        private readonly IApplicationManager _applicationManager;
        private readonly IApproveManager _approveManager;
        private readonly IFacultyManager _facultyManager;
        private readonly IGroupManager _groupManager;
        private readonly IOrganizationManager _organizationManager;
        private readonly IParticipantManager _participantManager;
        private readonly IPracticeManager _practiceManager;
        private readonly IUserAccountManager _userAccountManager;

        private readonly IContractManager _contractManager;
        private readonly IStudyFieldManager _studyFieldManager;
        private readonly IStudyPlanManager _studyPlanManager;
        private readonly IProxyManager _proxyManager;

        public DocumentController(
            IApplicationManager applicationManager,
            IApproveManager approveManager,
            IFacultyManager facultyManager,
            IGroupManager groupManager,
            IOrganizationManager organizationManager,
            IParticipantManager participantManager,
            IPracticeManager practiceManager,
            IUserAccountManager userAccountManager,
            IContractManager contractManager,
            IStudyFieldManager studyFieldManager,
            IStudyPlanManager studyPlanManager,
            IProxyManager proxyManager)
        {
            _applicationManager = applicationManager;
            _approveManager = approveManager;
            _facultyManager = facultyManager;
            _groupManager = groupManager;
            _organizationManager = organizationManager;
            _participantManager = participantManager;
            _practiceManager = practiceManager;
            _userAccountManager = userAccountManager;
            _contractManager = contractManager;
            _studyFieldManager = studyFieldManager;
            _studyPlanManager = studyPlanManager;
            _proxyManager = proxyManager;
        }

        [HttpGet]
        [Route("applications/{id:guid}/download")]
        [Authorize(Roles = "student")]
        public async Task<ActionResult> DownloadApplication(Guid id)
        {
            var userId = Guid.Parse(Request.Cookies["uid"]!);

            var application = _applicationManager.Find<ApplicationEntity>(id);
            if (!application.Success)
                return BadRequest();

            var student = _participantManager.Find<StudentEntity>(application.Value!.StudentId);
            if (!student.Success)
                return BadRequest();

            if (userId != student.Value!.UserId)
                return Forbid();

            var userSt = (await _userAccountManager.FindUserAsync(userId)).Value!;
            var group = _groupManager.Find<GroupEntity>(student.Value!.GroupId).Value!;
            var department = _facultyManager.Find<DepartmentEntity>(group.DepartmentId).Value!;
            var departmentHead = _participantManager.GetLastestByDepartmentId<DepartmentHeadEntity>(group.DepartmentId).Value!;
            var userDH = (await _userAccountManager.FindUserAsync(departmentHead.UserId)).Value!;

            var date = _practiceManager.Find<PracticeDateEntity>(application.Value!.PracticeDateId).Value!;
            var approvedPractice = _approveManager.Find<ApprovedPracticeEntity>(date.ApprovedPracticeId).Value!;
            var practice = _practiceManager.Find<PracticeEntity>(approvedPractice.PracticeId).Value!;
            var type = _practiceManager.Find<PracticeTypeEntity>(practice.PracticeTypeId).Value?.Name;
            var kind = _practiceManager.Find<PracticeKindEntity>(practice.PracticeKindId).Value?.Name;

            var supervisor = _participantManager
                .GetBy<SupervisorEntity>(entity => entity.PracticeDateId == date.Id).Value!
                .FirstOrDefault(entity => entity.GroupId == group.Id && entity.IsHead);

            var teacher = _participantManager.Find<TeacherEntity>(supervisor!.TeacherId).Value!;
            var userTeacher = (await _userAccountManager.FindUserAsync(teacher.UserId)).Value!;

            var organization = _organizationManager.Find<OrganizationEntity>((Guid)application.Value!.OrganizationId!).Value!;
            

            return GenerateApplication(new GenerateApplicationServiceModel
            {
                DepartmentAbbr = $"{ department.Abbreviation }",
                DepartmentHeadName = $"{userDH.LastName}" + $" {userDH.Name[0]}." + (userDH.Patronymic == null ? "" : $"{userDH.Patronymic[0]}."),
                GroupName = group.Name,
                StudentName = $"{userSt.LastName} {userSt.Name} {userSt.Patronymic}",
                PracticeKind = $"{kind}",
                PracticeType = $"{type}",
                OrganizationName = organization.OrganizationName,
                OrganizationAddress = organization.OrganizationAddress,
                CurrentDate = DateTime.UtcNow.Date.ToString("dd.MM.yyyy"),
                StartDate = date.StartsAt.ToString("dd.MM.yyyy"),
                EndDate = date.EndsAt.ToString("dd.MM.yyyy"),
                SupervisorName = $"{userTeacher.LastName}" + $" {userTeacher.Name[0]}." + (userTeacher.Patronymic == null ? "" : $"{userTeacher.Patronymic[0]}.")
            });
        }

        [HttpGet]
        [Route("contracts/{id:guid}/download")]
        public async Task<ActionResult> DownloadContract(Guid id)
        {
            var contract = _contractManager.Find<ContractEntity>(id);
            if (!contract.Success)
                return BadRequest();

            var organization = _organizationManager.Find<OrganizationEntity>(contract.Value!.OrganizationId).Value!;
            var profiles = _contractManager.GetBy<PracticeProfileEntity>(entity => entity.ContractId == id).Value!;

            var student = _participantManager.Find<StudentEntity>(profiles.FirstOrDefault()!.StudentId).Value!;
            var group = _groupManager.Find<GroupEntity>(student.GroupId).Value!;

            var supervisor = _participantManager.GetBy<SupervisorEntity>(entity => entity.GroupId == group.Id && entity.PracticeDateId == profiles.FirstOrDefault()!.PracticeDateId).Value!.FirstOrDefault()!;
            var teacher = _participantManager.Find<TeacherEntity>(entity => entity.Id == supervisor.TeacherId).Value!;
            var userSupervisor = (await _userAccountManager.FindUserAsync(supervisor.Id = teacher.UserId)).Value!;

            var proxies = _proxyManager.GetAll<ProxyEntity>();
            var proxy = proxies.Value?.MaxBy(entity => entity.ValidFrom);

            var signatory = proxy != null
                ? _proxyManager.GetBy<SignatoryEntity>(entity => entity.ProxyId == proxy.Id).Value?.FirstOrDefault()
                : null;

            var userSignatory = signatory != null
                ? await _userAccountManager.FindUserAsync(signatory.UserId)
                : null;

            var students = new List<GenerateContractServiceModel.StudentInformation>();
            foreach (var profile in profiles)
            {
                var date = _practiceManager.Find<PracticeDateEntity>(profile.PracticeDateId).Value!;
                var approvedPractice = _approveManager.Find<ApprovedPracticeEntity>(date.ApprovedPracticeId).Value!;
                var practice = _practiceManager.Find<PracticeEntity>(approvedPractice.PracticeId).Value!;
                var type = _practiceManager.Find<PracticeTypeEntity>(practice.PracticeTypeId).Value!;
                var kind = _practiceManager.Find<PracticeKindEntity>(practice.PracticeKindId).Value!;

                var st = _participantManager.Find<StudentEntity>(profile.StudentId).Value!;
                var gr = _groupManager.Find<GroupEntity>(st.GroupId).Value!;
                var approvedStudyPlan = _approveManager.Find<ApprovedStudyPlanEntity>(gr.ApprovedStudyPlanId).Value!;
                var studyPlan = _studyPlanManager.Find<StudyPlanEntity>(approvedStudyPlan.StudyPlanId).Value!;
                var degree = _studyPlanManager.Find<DegreeEntity>(studyPlan.DegreeId).Value!;
                var studyField = _studyFieldManager.Find<StudyFieldEntity>(studyPlan.StudyFieldId).Value!;
                var specialty = _studyFieldManager.Find<SpecialtyEntity>(studyField.SpecialtyId).Value!;

                var userSt = (await _userAccountManager.FindUserAsync(st.UserId)).Value!;

                students.Add(new GenerateContractServiceModel.StudentInformation
                {
                    Year = (DateTime.UtcNow.Year - approvedStudyPlan.Year + 
                            (DateTime.UtcNow > new DateTime(DateTime.UtcNow.Year, 09, 01)
                            ? 1 : 0)).ToString(),
                    GroupName = group.Name,
                    StudentName = $"{userSt.LastName} {userSt.Name} {userSt.Patronymic}",
                    PracticeKind = kind.Name,
                    PracticeType = type.Name,
                    StartDate = date.StartsAt.ToString("dd.MM.yyyy"),
                    EndDate = date.EndsAt.ToString("dd.MM.yyyy"),
                    StudyFieldName = studyField.Name,
                    StudyFieldCode = studyField.Code,
                    SpecialtyName = specialty.Name
                });
            }

            return GenerateContract(new GenerateContractServiceModel
            {
                SignatoryName = $"{userSignatory?.Value?.LastName}" + $" {userSignatory?.Value?.Name[0]}." + (userSignatory?.Value?.Patronymic != null ? $"{userSignatory?.Value?.Patronymic[0]}." : ""),
                SupervisorName = $"{userSupervisor?.LastName}" + $" {userSupervisor?.Name[0]}." + (userSupervisor?.Patronymic != null ? $"{userSupervisor?.Patronymic[0]}." : ""),
                ProxyStartDate = $"{proxy?.ValidFrom.ToString("dd.MM.yyyy")}",
                ProxyNumber = "20/330",
                OrganizationName = organization.OrganizationName,
                OrganizationAddress = organization.OrganizationAddress
            }, students);
        }

        private ActionResult GenerateApplication(GenerateApplicationServiceModel model)
        {
            var documentPath = Path.Combine(Directory.GetCurrentDirectory(), "Static\\application.docx");
            var replacements = CreateReplacement(model);

            var app = new Microsoft.Office.Interop.Word.Application();
            app.Documents.Open(documentPath);

            GenerateDocument(ref app, replacements);

            var tmpName = documentPath.Replace(".docx", "-tmp.docx");
            var tmpFile = Path.Combine(tmpName);

            app.ActiveDocument.SaveAs2(tmpFile);
            app.ActiveDocument.Close();
            app.Quit();

            var file = System.IO.File.ReadAllBytes(tmpFile);
            System.IO.File.Delete(tmpFile);

            return File(file, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", $"Заявление.docx");
        }

        private ActionResult GenerateContract(GenerateContractServiceModel model, List<GenerateContractServiceModel.StudentInformation> students)
        {
            var documentPath = Path.Combine(Directory.GetCurrentDirectory(), "Static\\contract.docx");
            var tmpName = documentPath.Replace(".docx", "-tmp.docx");

            var replacements = CreateReplacement(model);

            var app = new Microsoft.Office.Interop.Word.Application();

            try
            {
                app.Documents.Open(documentPath);

                GenerateDocument(ref app, replacements);

                var table = app.ActiveDocument.Tables[3];
                for (var i = 2; i < 2 + students.Count(); i++)
                {
                    var current = students[i - 2];

                    if (students.FindIndex(entity => entity.StudyFieldCode == current.StudyFieldCode) == i - 2)
                    {
                        var find = app.Selection.Find;

                        var wrap = Microsoft.Office.Interop.Word.WdFindWrap.wdFindContinue;
                        var replace = Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne;

                        var findText = "[StudyFieldCode]";
                        var replaceWith = current.StudyFieldCode;

                        find.Execute(FindText: findText, ReplaceWith: replaceWith, Wrap: wrap, Replace: replace);

                        findText = "[StudyFieldName]";
                        replaceWith = current.StudyFieldName;

                        if (
                            i - 2 != students.Count() - 1
                            && students.FindLastIndex(entity => entity.StudyFieldCode != current.StudyFieldCode) > i - 2)
                        {
                            replaceWith += ", [StudyFieldCode] - «[StudyFieldName]»";
                        }

                        find.Execute(FindText: findText, ReplaceWith: replaceWith, Wrap: wrap, Replace: replace);
                    }

                    table.Rows.Add();
                    table.Cell(i, 1).Range.Text = $"{current.StudyFieldCode} {current.StudyFieldName}, {current.SpecialtyName}";
                    table.Cell(i, 2).Range.Text = $"{current.PracticeKind}: {current.PracticeType}";
                    table.Cell(i, 3).Range.Text = "1";
                    table.Cell(i, 4).Range.Text = $"{current.StudentName}, {current.Year} курс, гр. {current.GroupName}";
                    table.Cell(i, 5).Range.Text = $"{current.StartDate} - {current.EndDate}";
                }
            }
            catch (Exception) 
            {
                app.ActiveDocument.SaveAs2(tmpName);
                app.ActiveDocument.Close();
                app.Quit();

                return BadRequest();
            }

            app.ActiveDocument.SaveAs2(tmpName);
            app.ActiveDocument.Close();
            app.Quit();

            var file = System.IO.File.ReadAllBytes(tmpName);
            System.IO.File.Delete(tmpName);

            return File(file, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", $"Договор.docx");
        }

        private void GenerateDocument(ref Microsoft.Office.Interop.Word.Application app, Dictionary<string, string> replacements)
        {
            foreach (var replacement in replacements)
            {
                var find = app.Selection.Find;

                var wrap = Microsoft.Office.Interop.Word.WdFindWrap.wdFindContinue;
                var replace = Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll;
                var findText = $"[{replacement.Key}]";
                var replaceWith = replacement.Value;

                find.Execute(FindText: findText, ReplaceWith: replaceWith, Wrap: wrap, Replace: replace);
            }
        }

        private static Dictionary<string, string> CreateReplacement<T>(T model)
        {
            var replacement = new Dictionary<string, string>();

            var fields = typeof(T).GetProperties();
            foreach (var field in fields)
            {
                replacement[field.Name] = 
                    field.GetValue(model)!.ToString()!;
            }

            return replacement;
        }
    }
}
