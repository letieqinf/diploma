using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Domain.Models.Result;
using Tusur.Practices.Application.Ports.Input.Utils;

namespace Tusur.Practices.Application.Ports.Input
{
    public interface IPropertyManager :
        IProperty<ContractEntity>,
        IProperty<PracticeProfileEntity>
    {
        public RequestResult IsInPropertyOf<TDomain>(Guid id, Guid userId, string role) where TDomain : DomainEntity;
        public RequestResult<IEnumerable<TDomain>> GetAllPropertyOf<TDomain>(Guid userId, string role) where TDomain : DomainEntity;
    }
}
