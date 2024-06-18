using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Domain.Models.Result;

namespace Tusur.Practices.Application.Ports.Input.Utils
{
    public interface IProperty<TDomain>
        where TDomain : DomainEntity
    {
        public RequestResult IsInPropertyOf(Guid id, Guid userId, string role);
        public RequestResult<IEnumerable<TDomain>> GetAllPropertyOf(Guid userId, string role);
    }
}
