using System.Linq.Expressions;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Domain.Models;
using Tusur.Practices.Application.Domain.Models.Result;
using Tusur.Practices.Application.Ports.Input;
using Tusur.Practices.Application.Ports.Input.Utils;
using Tusur.Practices.Application.Ports.Output;

namespace Tusur.Practices.Application.UseCases
{
    public class ContractManager : IContractManager
    {
        IService<ContractEntity> IManager<ContractEntity>.Service { get; set; }
        IService<ContractContentEntity> IManager<ContractContentEntity>.Service { get; set; }
        IService<PracticeProfileEntity> IManager<PracticeProfileEntity>.Service { get; set; }

        public ContractManager(
            IService<ContractEntity> contractService,
            IService<ContractContentEntity> contractContentService,
            IService<PracticeProfileEntity> practiceProfileService)
        {
            ((IManager<ContractEntity>)this).Service = contractService;
            ((IManager<ContractContentEntity>)this).Service = contractContentService;
            ((IManager<PracticeProfileEntity>)this).Service = practiceProfileService;
        }

        public RequestResult<ContractContentEntity> GetDefault<TDomain>() where TDomain : ContractContentEntity
        {
            var contents = ((IManager<ContractContentEntity>)this).Service.GetBy(entity => entity.IsDefault == true);

            if (!contents.Success)
                return new RequestResult<ContractContentEntity> { Success = false, Error = contents.Error };

            var defaultContent = contents.Value!.FirstOrDefault();
            if (defaultContent == null)
                return new RequestResult<ContractContentEntity> { Success = false, Error = new NullReferenceException().Message };

            return new RequestResult<ContractContentEntity> { Success = true, Value = defaultContent };
        }
    }
}
