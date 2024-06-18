using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Ports.Input;
using Tusur.Practices.Application.UseCases;
using Tusur.Practices.Server.Models.Request;

namespace Tusur.Practices.Server.Controllers
{
    [Route("api/supervisors")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SupervisorController : ControllerBase
    {
        private readonly IParticipantManager _participantManager;
        private readonly IUserAccountManager _userAccountManager;
        private readonly IUserPracticeManager _userPracticeManager;

        public SupervisorController(IParticipantManager participantManager, IUserAccountManager userAccountManager, IUserPracticeManager userPracticeManager)
        {
            _participantManager = participantManager;
            _userAccountManager = userAccountManager;
            _userPracticeManager = userPracticeManager;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> GetAllSupervisors(string role, Guid? practiceDateId, Guid? groupId)
        {
            var userId = Guid.Parse(Request.Cookies["uid"]!);

            var isInRole = await _userAccountManager.IsUserInRoleAsync(userId, role);
            if (!isInRole.Success)
                return Forbid();

            var supervisors = _userPracticeManager.GetAllUserSupervisors(userId, role);
            if (!supervisors.Success)
                return BadRequest();

            var result = supervisors.Value!;

            if (practiceDateId != null)
                result = result.Where(entity => entity.PracticeDateId == practiceDateId);

            if (groupId != null)
                result = result.Where(entity => entity.GroupId == groupId);

            return Ok(result);
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = "education")]
        public IActionResult CreateSupervisor([FromBody] CreateSupervisorRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var supervisor = _participantManager.Create<SupervisorEntity>(new SupervisorEntity
            {
                GroupId = model.GroupId,
                PracticeDateId = model.PracticeDateId,
                TeacherId = model.TeacherId,
                IsHead = true
            });

            if (!supervisor.Success)
                return BadRequest();

            return Ok();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "education")]
        public IActionResult RemoveSupervisor([FromRoute] Guid id)
        {
            var supervisor = _participantManager.Remove<SupervisorEntity>(id);
            if (!supervisor.Success)
                return BadRequest();

            return Ok();
        }
    }
}
