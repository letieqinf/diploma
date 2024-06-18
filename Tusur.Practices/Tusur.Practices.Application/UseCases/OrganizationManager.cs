using System.Linq.Expressions;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Domain.Models.Result;
using Tusur.Practices.Application.Ports.Input;
using Tusur.Practices.Application.Ports.Output;

namespace Tusur.Practices.Application.UseCases
{
    public class OrganizationManager : IOrganizationManager
    {
        IService<OrganizationEntity> IManager<OrganizationEntity>.Service { get; set; }

        public OrganizationManager(IService<OrganizationEntity> organizationService)
        {
            ((IManager<OrganizationEntity>)this).Service = organizationService;
        }
    }
}
