using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Ports.Input;
using Tusur.Practices.Server.Models.Response;

namespace Tusur.Practices.Server.Controllers
{
    [Route("api/practices")]
    [ApiController]

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PracticeController : ControllerBase
    {
        private readonly IPracticeManager _practiceManager;
        private readonly IApproveManager _approveManager;
        private readonly IGroupManager _groupManager;
        private readonly IUserPracticeManager _userPracticeManager;
        private readonly IUserAccountManager _userAccountManager;

        public PracticeController(
            IPracticeManager practiceManager,
            IApproveManager approveManager,
            IGroupManager groupManager,
            IUserPracticeManager userPracticeManager,
            IUserAccountManager userAccountManager)
        {
            _practiceManager = practiceManager;
            _approveManager = approveManager;
            _groupManager = groupManager;
            _userPracticeManager = userPracticeManager;
            _userAccountManager = userAccountManager;
        }

        [HttpGet]
        [Route("")]
        public ActionResult GetAllPractices()
        {
            var result = _practiceManager.GetAll<PracticeEntity>();
            if (!result.Success)
                return BadRequest();

            var practices = new List<GetPracticeResponseModel>();
            foreach (var value in result.Value!)
            {
                (var type, var kind) = GetInformation(value);
                practices.Add(new GetPracticeResponseModel { Id = value.Id, Type = type, Kind = kind });
            }

            return Ok(practices);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public ActionResult GetById([FromRoute] Guid id)
        {
            var practiceResult = _practiceManager.Find<PracticeEntity>(id);
            if (!practiceResult.Success)
                return BadRequest();

            var practice = practiceResult.Value!;
            (var type, var kind) = GetInformation(practice);

            return Ok(new GetPracticeResponseModel
            {
                Id = practice.Id,
                Type = type,
                Kind = kind
            });
        }

        [HttpGet]
        [Route("dates")]
        public async Task<ActionResult> GetAllDates(string role, Guid? groupId)
        {
            var userId = Guid.Parse(Request.Cookies["uid"]!);

            ApprovedStudyPlanEntity? approvedStudyPlan = null;
            if (groupId != null)
            {
                var groupCheck = _groupManager.Find<GroupEntity>((Guid)groupId);
                if (!groupCheck.Success)
                    return BadRequest();

                var approvedStudyPlanCheck = _approveManager.Find<ApprovedStudyPlanEntity>(groupCheck.Value!.ApprovedStudyPlanId);
                if (!approvedStudyPlanCheck.Success)
                    return BadRequest();

                approvedStudyPlan = approvedStudyPlanCheck.Value!;
            }

            var isInRole = await _userAccountManager.IsUserInRoleAsync(userId, role);
            if (!isInRole.Success)
                return Forbid();

            var result = _userPracticeManager.GetAllUserDates(userId, role);
            if (!result.Success)
                return BadRequest();

            var dates = new List<GetDateResponseModel>();
            foreach (var value in result.Value!)
            {
                if (approvedStudyPlan != null)
                {
                    if (value.ApprovedStudyPlanId != approvedStudyPlan.Id)
                        continue;
                }

                var approvedResult = _approveManager.Find<ApprovedPracticeEntity>(value.ApprovedPracticeId);
                if (!approvedResult.Success)
                    continue;

                var practiceResult = _practiceManager.Find<PracticeEntity>(approvedResult.Value!.PracticeId);
                if (!practiceResult.Success)
                    continue;

                (var type, var kind) = GetInformation(practiceResult.Value!);
                dates.Add(new GetDateResponseModel { Id = value.Id, Kind = kind, Type = type, StartsAt = value.StartsAt, EndsAt = value.EndsAt });
            }

            return Ok(dates);
        }

        [HttpGet]
        [Route("dates/{id:guid}")]
        public ActionResult GetDateById([FromRoute] Guid id)
        {
            var date = _practiceManager.Find<PracticeDateEntity>(id);
            if (!date.Success)
                return BadRequest();

            var approvedResult = _approveManager.Find<ApprovedPracticeEntity>(date.Value!.ApprovedPracticeId);
            if (!approvedResult.Success)
                return BadRequest();

            var practiceResult = _practiceManager.Find<PracticeEntity>(approvedResult.Value!.PracticeId);
            if (!practiceResult.Success)
                return BadRequest();

            (var type, var kind) = GetInformation(practiceResult.Value!);

            return Ok(new GetDateResponseModel
            {
                Id = date.Value!.Id,
                Type = type,
                Kind = kind,
                StartsAt = date.Value!.StartsAt,
                EndsAt = date.Value!.EndsAt
            });
        }

        private (string? type, string? kind) GetInformation(PracticeEntity practice)
        {
            var type = _practiceManager.Find<PracticeTypeEntity>(practice.PracticeTypeId).Value?.Name;
            var kind = _practiceManager.Find<PracticeKindEntity>(practice.PracticeKindId).Value?.Name;

            return (type, kind);
        }
    }
}
