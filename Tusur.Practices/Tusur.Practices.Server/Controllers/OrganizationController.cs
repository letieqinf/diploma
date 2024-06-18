using Microsoft.AspNetCore.Mvc;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Ports.Input;
using Tusur.Practices.Server.Models.Request;

namespace Tusur.Practices.Server.Controllers
{
    [Route("api/organizations")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationManager _organizationManager;

        public OrganizationController(IOrganizationManager organizationManager)
        {
            _organizationManager = organizationManager;
        }

        [HttpGet]
        [Route("")]
        public ActionResult GetAll(bool approvedOnly)
        {
            var organizations = _organizationManager.GetAll<OrganizationEntity>();
            if (!organizations.Success)
                return BadRequest();

            var result = !approvedOnly ? organizations.Value! : organizations.Value!.Where(entity => entity.IsApproved);
            return Ok(result);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public ActionResult GetById(Guid id)
        {
            var organization = _organizationManager.Find<OrganizationEntity>(id);
            if (!organization.Success)
                return BadRequest();

            return Ok(organization.Value);
        }

        [HttpPatch]
        [Route("{id:guid}/approve")]
        public ActionResult Approve(Guid id, [FromBody] ApproveOrganizationRequestModel model)
        {
            var organization = _organizationManager.Find<OrganizationEntity>(id);
            if (!organization.Success)
                return BadRequest();

            organization.Value!.Inn = model.Inn;
            organization.Value!.Trrc = model.Trrc;
            organization.Value!.IsApproved = true;

            var result = _organizationManager.Update(organization.Value!);
            if (!result.Success)
                return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok();
        }

        [HttpGet]
        [Route("{inn:long}/{trrc:long}")]
        public ActionResult GetByInnAndTrrc(long inn, long trrc)
        {
            var organizations = _organizationManager.GetBy<OrganizationEntity>(entity => entity.Inn == inn && entity.Trrc == trrc);
            if (!organizations.Success)
                return BadRequest();

            var organization = organizations.Value!.FirstOrDefault();
            return Ok(organization);
        }
    }
}
