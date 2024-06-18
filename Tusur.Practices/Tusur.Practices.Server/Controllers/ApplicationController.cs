using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Domain.Models;
using Tusur.Practices.Application.Domain.Models.Result;
using Tusur.Practices.Application.Ports.Input;
using Tusur.Practices.Server.Models.Request;
using Tusur.Practices.Server.Models.Response;

namespace Tusur.Practices.Server.Controllers
{
    [Route("api/applications")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationManager _applicationManager;
        private readonly IOrganizationManager _organizationManager;
        private readonly IParticipantManager _participantManager;
        private readonly IUserPracticeManager _userPracticeManager;

        public ApplicationController(
            IApplicationManager applicationManager,
            IOrganizationManager organizationManager, 
            IParticipantManager participantManager, 
            IUserPracticeManager userPracticeManager)
        {
            _applicationManager = applicationManager;
            _organizationManager = organizationManager;
            _participantManager = participantManager;
            _userPracticeManager = userPracticeManager;
        }

        [HttpGet]
        [Route("")]
        public ActionResult<IEnumerable<GetApplicationResponseModel>> GetAllApplications([FromQuery] string role)
        {
            var userId = Guid.Parse(Request.Cookies["uid"]!);

            var applications = _userPracticeManager.GetAllUserApplications(userId, role);
            if (!applications.Success)
                return BadRequest();

            return Ok(applications.Value!.Select(entity => new GetApplicationResponseModel
            { 
                Id = entity.Id,
                StudentId = entity.StudentId,
                PracticeDateId = entity.PracticeDateId,
                OrganizationId = entity.OrganizationId,
                Status = entity.Status
            }));
        }

        [HttpGet]
        [Route("filtered")]
        [Authorize(Roles = "teacher")]
        public ActionResult<IEnumerable<GetApplicationResponseModel>> GetApplicationsByPracticeDateId([FromQuery] Guid practiceDateId, [FromQuery] Guid? groupId)
        {
            var userId = Guid.Parse(Request.Cookies["uid"]!);

            var supervisors = _userPracticeManager.GetAllUserSupervisors(userId, DomainDefaults.Teacher);
            if (!supervisors.Success)
                return BadRequest();

            var supervisorsOnPractice = supervisors.Value!.Where(entity => entity.PracticeDateId == practiceDateId);
            if (supervisorsOnPractice == null)
                return Forbid();

            var result = new List<GetApplicationResponseModel>();

            foreach (var supervisor in supervisorsOnPractice)
            {
                var students = _participantManager.GetBy<StudentEntity>(entity => entity.GroupId == supervisor.GroupId);
                if (!students.Success)
                    continue;

                if (groupId != null)
                    students.Value = students.Value!.Where(entity => entity.GroupId == groupId);

                foreach (var student in students.Value!)
                {
                    var applications = _applicationManager.GetBy<ApplicationEntity>(entity => entity.StudentId == student.Id && entity.PracticeDateId == practiceDateId);
                    if (!applications.Success)
                        continue;

                    var application = applications.Value!.FirstOrDefault();
                    if (application == null)
                        continue;

                    if (application!.Status == 0)
                        continue;

                    result.Add(new GetApplicationResponseModel
                    {
                        Id = application!.Id,
                        StudentId = application.StudentId,
                        PracticeDateId = application.PracticeDateId,
                        OrganizationId = application.OrganizationId,
                        Status = application.Status
                    });
                }
            }

            return new ActionResult<IEnumerable<GetApplicationResponseModel>>(result);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public ActionResult GetApplicationById([FromRoute] Guid id)
        {
            return Ok();
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = "student")]
        public ActionResult CreateApplication([FromBody] CreateApplicationRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var userId = Guid.Parse(Request.Cookies["uid"]!);

            var studentResult = _participantManager.GetLastestByUserId<StudentEntity>(userId);
            if (!studentResult.Success)
                return BadRequest();

            RequestResult<ApplicationEntity>? result;
            if (model.OrganizationId == null)
            {
                var organizationResult = _organizationManager.Create(new OrganizationEntity
                {
                    OrganizationName = model.OrganizationName!,
                    OrganizationAddress = model.OrganizationAddress!,
                    IsApproved = false
                });

                if (!organizationResult.Success)
                    return BadRequest();

                result = _applicationManager.Create(new ApplicationEntity
                {
                    OrganizationId = organizationResult.Value!.Id,
                    PracticeDateId = model.PracticeDateId,
                    StudentId = studentResult.Value!.Id,
                    Status = (ushort)(model.IsDraft ? 0 : 1)
                });

                if (!result.Success)
                {
                    _organizationManager.Remove<OrganizationEntity>(organizationResult.Value.Id);
                    return BadRequest();
                }

                return Ok();
            }

            result = _applicationManager.Create(new ApplicationEntity
            {
                OrganizationId = model.OrganizationId,
                PracticeDateId = model.PracticeDateId,
                StudentId = studentResult.Value!.Id,
                Status = (ushort)(model.IsDraft ? 0 : 1)
            });

            if (!result.Success)
                return BadRequest();

            return Ok();
        }

        [HttpPatch]
        [Route("{id:guid}/submit")]
        public ActionResult SubmitApplication([FromRoute] Guid id)
        {
            var application = _applicationManager.Find<ApplicationEntity>(id);
            if (!application.Success)
                return BadRequest();

            if (application.Value!.Status != 0)
                return BadRequest();

            application.Value!.Status = 1;

            var result = _applicationManager.Update(application.Value!);
            if (!result.Success)
                return BadRequest();

            return Ok();
        }

        [HttpPatch]
        [Route("{id:guid}")]
        public ActionResult EditApplication([FromRoute] Guid id, [FromBody] object model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return Ok();
        }

        [HttpPatch]
        [Route("{id:guid}/approve")]
        public ActionResult ApproveApplication(Guid id)
        {
            var application = _applicationManager.Find<ApplicationEntity>(id);
            if (!application.Success)
                return BadRequest();

            if (application.Value!.Status != 1)
                return Forbid();

            application.Value!.Status = 2;

            var appApprove = _applicationManager.Update(application.Value!);
            if (!appApprove.Success)
                return BadRequest();

            return Ok();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public ActionResult DeleteApplication([FromRoute] Guid id)
        {
            var application = _applicationManager.Remove<ApplicationEntity>(id);
            if (!application.Success)
                return BadRequest();

            return Ok(); 
        }
    }
}
