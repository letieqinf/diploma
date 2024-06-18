using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Xml.Linq;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Domain.Models;
using Tusur.Practices.Application.Ports.Input;
using Tusur.Practices.Server.Models.Request;
using Tusur.Practices.Server.Models.Response;

namespace Tusur.Practices.Server.Controllers
{
    [Route("api/practice-profiles")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ContractController : ControllerBase
    {
        private readonly IContractManager _contractManager;
        private readonly IPropertyManager _propertyManager;

        public ContractController(IContractManager contractManager, IPropertyManager propertyManager)
        {
            _contractManager = contractManager;
            _propertyManager = propertyManager;
        }

        [HttpGet]
        [Route("")]
        [Authorize(Roles = "teacher,secretary,education")]
        public ActionResult<IEnumerable<GetProfilesResponse>> GetAllProfiles([FromQuery] string role)
        {
            var userId = Guid.Parse(Request.Cookies["uid"]!);

            var profiles = _propertyManager.GetAllPropertyOf<PracticeProfileEntity>(userId, role);
            if (!profiles.Success)
                return BadRequest();

            var result = new List<GetProfilesResponse>();

            foreach (var profile in profiles.Value!)
            {
                var index = result.FindIndex(entity => entity.ContractId == profile.ContractId);
                if (index == -1)
                {
                    var element = new GetProfilesResponse
                    {
                        ContractId = profile.ContractId,
                        StudentDates = new List<GetProfileStudentDate>()
                        {

                        },
                        Status = profile.Status
                    };
                    element.StudentDates = element.StudentDates
                        .Append(new GetProfileStudentDate { StudentId = profile.StudentId, PracticeDateId = profile.PracticeDateId });

                    result.Add(element);

                    continue;
                }

                result[index].StudentDates = result[index].StudentDates
                    .Append(new GetProfileStudentDate { StudentId = profile.StudentId, PracticeDateId = profile.PracticeDateId });
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("contracts/{id:guid}")]
        [Authorize(Roles = "teacher,education")]
        public IActionResult GetContract([FromRoute] Guid id, [FromQuery] string role)
        {
            var userId = Guid.Parse(Request.Cookies["uid"]!);

            var contract = _contractManager.Find<ContractEntity>(id);
            if (!contract.Success)
                return BadRequest();

            var check = _propertyManager.IsInPropertyOf<ContractEntity>(contract.Value!.Id, userId, role);
            if (!check.Success)
                return Forbid();

            return Ok(contract.Value!);
        }

        [HttpPost]
        [Route("contracts")]
        [Authorize(Roles = "teacher")]
        public IActionResult CreateContract(CreateContractRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var content = _contractManager.GetDefault<ContractContentEntity>();
            if (!content.Success)
                return BadRequest();

            var contract = new ContractEntity
            {
                ContractContentId = content.Value!.Id,
                OrganizationId = model.OrganizationId,
                IsLongTerm = false
            };

            var creationResult = _contractManager.Create(contract);
            if (!creationResult.Success)
                return BadRequest();

            foreach (var student in model.StudentDates)
            {
                var profile = new PracticeProfileEntity
                {
                    ContractId = creationResult.Value!.Id,
                    StudentId = student.StudentId,
                    PracticeDateId = student.PracticeDateId,
                    Status = (ushort)(model.IsDraft ? 0 : 1)
                };

                var profileCreationResult = _contractManager.Create(profile);
                if (!profileCreationResult.Success)
                    continue;
            }

            return Ok();
        }

        [HttpPatch]
        [Route("")]
        [Authorize(Roles = "teacher,education")]
        public IActionResult SubmitProfile([FromBody] SubmitContractRequestModel model, [FromQuery] string role)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var userId = Guid.Parse(Request.Cookies["uid"]!);

            var checkProperty = _propertyManager.IsInPropertyOf<ContractEntity>(model.ContractId, userId, role);
            if (!checkProperty.Success)
                return Forbid();

            var profiles = _contractManager
                    .GetBy<PracticeProfileEntity>(entity => entity.ContractId == model.ContractId && entity.Status != 2);

            if (!profiles.Success)
                return NotFound();

            foreach (var studentDate in model.StudentDates)
            {
                var profile = profiles.Value!
                    .FirstOrDefault(entity => entity.StudentId == studentDate.StudentId && entity.PracticeDateId == studentDate.PracticeDateId);

                if (profile == null)
                    continue;

                profile.Status++;
                var result = _contractManager.Update(profile);

                if (!result.Success)
                    continue;
            }

            return Ok();
        }


        [HttpPut]
        [Route("")]
        [Authorize(Roles = "teacher")]
        public IActionResult RemoveProfile([FromBody] SubmitContractRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var userId = Guid.Parse(Request.Cookies["uid"]!);

            var checkProperty = _propertyManager.IsInPropertyOf<ContractEntity>(model.ContractId, userId, DomainDefaults.Teacher);
            if (!checkProperty.Success)
                return Forbid();

            var profiles = _contractManager
                    .GetBy<PracticeProfileEntity>(entity => entity.ContractId == model.ContractId);

            if (!profiles.Success)
                return BadRequest();

            foreach (var studentDate in model.StudentDates)
            {
                var profile = profiles.Value!
                    .FirstOrDefault(entity => entity.StudentId == studentDate.StudentId && entity.PracticeDateId == studentDate.PracticeDateId);

                if (profile == null)
                    continue;

                var result = _contractManager.Remove<PracticeProfileEntity>(profile.Id);
                if (!result.Success)
                    continue;
            }

            return Ok();
        }
    }
}
