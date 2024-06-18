using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Ports.Input;
using Tusur.Practices.Persistence.Database.Entities;
using Tusur.Practices.Server.Models.Response;

namespace Tusur.Practices.Server.Controllers
{
    [Route("api/groups")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GroupController : ControllerBase
    {
        private readonly IApproveManager _approveManager;
        private readonly IFacultyManager _facultyManager;
        private readonly IGroupManager _groupManager;
        private readonly IParticipantManager _participantManager;
        private readonly IStudyFieldManager _studyFieldManager;
        private readonly IStudyPlanManager _studyPlanManager;
        private readonly IUserAccountManager _userAccountManager;

        public GroupController(
            IApproveManager approveManager,
            IFacultyManager facultyManager,
            IGroupManager groupManager,
            IParticipantManager participantManager,
            IStudyFieldManager studyFieldManager,
            IStudyPlanManager studyPlanManager,
            IUserAccountManager userAccountManager)
        {
            _approveManager = approveManager;
            _facultyManager = facultyManager;
            _groupManager = groupManager;
            _participantManager = participantManager;
            _studyFieldManager = studyFieldManager;
            _studyPlanManager = studyPlanManager;
            _userAccountManager = userAccountManager;
        }

        [HttpGet]
        [Route("")]
        public ActionResult<IEnumerable<GetGroupResponse>> GetAll()
        {
            var groups = _groupManager.GetAll<GroupEntity>();
            if (!groups.Success)
                return BadRequest();

            var result = new List<GetGroupResponse>();
            foreach (var group in groups.Value!)
            {
                var element = new GetGroupResponse
                {
                    Id = group.Id,
                    Name = group.Name,
                    DepartmentId = group.DepartmentId
                };

                var approvedStudyPlan = _approveManager.Find<ApprovedStudyPlanEntity>(group.ApprovedStudyPlanId);
                if (approvedStudyPlan.Success)
                {
                    element.ApprovedStudyPlanId = approvedStudyPlan.Value!.Id;
                    element.Year = approvedStudyPlan.Value!.Year;

                    var studyPlan = _studyPlanManager.Find<StudyPlanEntity>(approvedStudyPlan.Value!.StudyPlanId);
                    if (studyPlan.Success)
                    {
                        var studyField = _studyFieldManager.Find<StudyFieldEntity>(studyPlan.Value!.StudyFieldId);
                        if (studyField.Success)
                        {
                            element.StudyFieldName = studyField.Value!.Name;
                            element.StudyFieldCode = studyField.Value!.Code;

                            var specialty = _studyFieldManager.Find<SpecialtyEntity>(studyField.Value!.SpecialtyId);
                            if (specialty.Success)
                                element.SpecialtyName = specialty.Value!.Name;
                        }
                    }
                }

                var department = _facultyManager.Find<DepartmentEntity>(group.DepartmentId);
                if (department.Success)
                {
                    element.DepartmentName = department.Value!.Name;
                    element.DepartmentAbbr = department.Value!.Abbreviation;

                    var faculty = _facultyManager.Find<FacultyEntity>(department.Value!.FacultyId);
                    if (faculty.Success)
                    {
                        element.FacultyName = faculty.Value!.Name;
                        element.FacultyAbbr = faculty.Value!.Abbreviation;
                    }
                }

                result.Add(element);
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public ActionResult<GetGroupResponse> GetById(Guid id)
        {
            var group = _groupManager.Find<GroupEntity>(id);
            if (!group.Success)
                return BadRequest();

            var result = new GetGroupResponse
            {
                Id = group.Value!.Id,
                Name = group.Value!.Name,
                DepartmentId = group.Value!.DepartmentId
            };

            var approvedStudyPlan = _approveManager.Find<ApprovedStudyPlanEntity>(group.Value!.ApprovedStudyPlanId);
            if (approvedStudyPlan.Success)
            {
                result.ApprovedStudyPlanId = approvedStudyPlan.Value!.Id;
                result.Year = approvedStudyPlan.Value!.Year;

                var studyPlan = _studyPlanManager.Find<StudyPlanEntity>(approvedStudyPlan.Value!.StudyPlanId);
                if (studyPlan.Success)
                {
                    var studyField = _studyFieldManager.Find<StudyFieldEntity>(studyPlan.Value!.StudyFieldId);
                    if (studyField.Success)
                    {
                        result.StudyFieldName = studyField.Value!.Name;
                        result.StudyFieldCode = studyField.Value!.Code;

                        var specialty = _studyFieldManager.Find<SpecialtyEntity>(studyField.Value!.SpecialtyId);
                        if (specialty.Success)
                            result.SpecialtyName = specialty.Value!.Name;
                    }
                }
            }

            var department = _facultyManager.Find<DepartmentEntity>(group.Value!.DepartmentId);
            if (department.Success)
            {
                result.DepartmentName = department.Value!.Name;
                result.DepartmentAbbr = department.Value!.Abbreviation;

                var faculty = _facultyManager.Find<FacultyEntity>(department.Value!.FacultyId);
                if (faculty.Success)
                {
                    result.FacultyName = faculty.Value!.Name;
                    result.FacultyAbbr = faculty.Value!.Abbreviation;
                }
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("students")]
        public async Task<ActionResult<IEnumerable<GetStudentsResponseModel>>> GetStudents()
        {
            var students = _participantManager.GetAll<StudentEntity>();
            if (!students.Success)
                return BadRequest();

            var users = new List<GetStudentsResponseModel>();
            foreach (var student in students.Value!)
            {
                var result = await _userAccountManager.FindUserAsync(entity => entity.Id == student.UserId);
                if (!result.Success)
                    continue;

                users.Add(new GetStudentsResponseModel
                {
                    Id = student.UserId,
                    StudentId = student.Id,
                    Email = result.Value!.Email,
                    Name = result.Value!.Name,
                    LastName = result.Value!.LastName,
                    Patronymic = result.Value!.Patronymic
                });
            }

            return Ok(users);
        }

        [HttpGet]
        [Route("students/{id:guid}")]
        public async Task<ActionResult<IEnumerable<GetStudentsResponseModel>>> GetStudentById(Guid id)
        {
            var student = _participantManager.Find<StudentEntity>(id);
            if (!student.Success)
                return BadRequest();

            var user = await _userAccountManager.FindUserAsync(entity => entity.Id == student.Value!.UserId);
            if (!user.Success)
                return BadRequest();

            var result = new GetStudentsResponseModel
            {
                Id = student.Value!.UserId,
                StudentId = student.Value!.Id,
                Email = user.Value!.Email,
                Name = user.Value!.Name,
                LastName = user.Value!.LastName,
                Patronymic = user.Value!.Patronymic
            };

            return Ok(result);
        }

        [HttpGet]
        [Route("{id:guid}/students")]
        public async Task<ActionResult<IEnumerable<GetStudentsResponseModel>>> GetStudents(Guid id)
        {
            var group = _groupManager.Find<GroupEntity>(id);
            if (!group.Success)
                return BadRequest();

            var students = _participantManager.GetBy<StudentEntity>(entity => entity.GroupId == id);
            if (!students.Success)
                return BadRequest();

            var users = new List<GetStudentsResponseModel>();
            foreach (var student in students.Value!)
            {
                var result = await _userAccountManager.FindUserAsync(entity => entity.Id == student.UserId);
                if (!result.Success)
                    continue;

                users.Add(new GetStudentsResponseModel
                {
                    Id = student.UserId,
                    StudentId = student.Id,
                    Email = result.Value!.Email,
                    Name = result.Value!.Name,
                    LastName = result.Value!.LastName,
                    Patronymic = result.Value!.Patronymic
                });
            }

            return Ok(users);
        }

        [HttpGet]
        [Route("{id:guid}/students/{studentId:guid}")]
        public async Task<ActionResult<GetStudentsResponseModel>> GetStudentById([FromRoute] Guid id, [FromRoute] Guid studentId)
        {
            var group = _groupManager.Find<GroupEntity>(id);
            if (!group.Success)
                return BadRequest();

            var students = _participantManager.GetBy<StudentEntity>(entity => entity.GroupId == id && entity.Id == studentId);
            if (!students.Success)
                return BadRequest();

            var student = students.Value!.FirstOrDefault()!;

            var user = await _userAccountManager.FindUserAsync(entity => entity.Id == student.UserId);
            if (!user.Success)
                return BadRequest();

            var result = new GetStudentsResponseModel
            {
                Id = student.UserId,
                StudentId = student.Id,
                Email = user.Value!.Email,
                Name = user.Value!.Name,
                LastName = user.Value!.LastName,
                Patronymic = user.Value!.Patronymic
            };

            return Ok(result);
        }

        [HttpGet]
        [Route("students/{studentId:guid}/full-information")]
        public async Task<ActionResult<GetStudentFullInfoResponseModel>> GetStudentFullInfoById([FromRoute] Guid studentId)
        {
            var student = _participantManager.Find<StudentEntity>(studentId);
            if (!student.Success)
                return NotFound();

            var user = await _userAccountManager.FindUserAsync(entity => entity.Id == student.Value!.UserId);
            if (!user.Success)
                return BadRequest();

            var result = new GetStudentFullInfoResponseModel
            {
                Id = student.Value!.UserId,
                StudentId = student.Value!.Id,
                Email = user.Value!.Email,
                Name = user.Value!.Name,
                LastName = user.Value!.LastName,
                Patronymic = user.Value!.Patronymic
            };

            var group = _groupManager.Find<GroupEntity>(student.Value!.GroupId);
            if (group.Success)
            {
                result.GroupName = group.Value!.Name;
                var department = _facultyManager.Find<DepartmentEntity>(group.Value!.DepartmentId);
                if (department.Success)
                {
                    result.DepartmentName = department.Value!.Name;
                    result.DepartmentAbbr = department.Value!.Abbreviation;

                    var faculty = _facultyManager.Find<FacultyEntity>(department.Value!.FacultyId);
                    if (faculty.Success)
                    {
                        result.FacultyName = faculty.Value!.Name;
                        result.FacultyAbbr = faculty.Value!.Abbreviation;
                    }
                }

                var approvedStudyPlan = _approveManager.Find<ApprovedStudyPlanEntity>(group.Value!.ApprovedStudyPlanId);
                if (approvedStudyPlan.Success)
                {
                    var studyPlan = _studyPlanManager.Find<StudyPlanEntity>(approvedStudyPlan.Value!.StudyPlanId);
                    if (studyPlan.Success)
                    {
                        var field = _studyFieldManager.Find<StudyFieldEntity>(studyPlan.Value!.StudyFieldId);
                        if (field.Success)
                        {
                            result.StudyFieldName = field.Value!.Name;
                            result.StudyFieldCode = field.Value!.Code;
                            
                            var specialty = _studyFieldManager.Find<SpecialtyEntity>(field.Value!.SpecialtyId);
                            if (specialty.Success)
                                result.SpecialtyName = specialty.Value!.Name;
                        }

                        var degree = _studyPlanManager.Find<DegreeEntity>(studyPlan.Value!.DegreeId);
                        if (degree.Success)
                            result.DegreeName = degree.Value!.Name;

                        var studyForm = _studyPlanManager.Find<StudyFormEntity>(studyPlan.Value!.StudyFromId);
                        if (studyForm.Success)
                            result.StudyFormName = studyForm.Value!.Name;
                    }
                }
            }

            return Ok(result);
        }
    }
}
