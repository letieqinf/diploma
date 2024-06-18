using System.Linq.Expressions;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Domain.Models.Result;
using Tusur.Practices.Application.Ports.Input.Utils;

namespace Tusur.Practices.Application.Ports.Input
{
    public interface IContractManager :
        IManager<ContractEntity>,
        IManager<ContractContentEntity>,
        IManager<PracticeProfileEntity>
    {
        public RequestResult<ContractContentEntity> GetDefault<TDomain>() where TDomain : ContractContentEntity;
    }
}
