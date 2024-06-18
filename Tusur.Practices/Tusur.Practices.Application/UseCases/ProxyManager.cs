using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Ports.Input;
using Tusur.Practices.Application.Ports.Output;

namespace Tusur.Practices.Application.UseCases
{
    public class ProxyManager : IProxyManager
    {
        IService<ProxyEntity> IManager<ProxyEntity>.Service { get; set; }
        IService<SignatoryEntity> IManager<SignatoryEntity>.Service { get; set; }

        public ProxyManager(
            IService<ProxyEntity> proxyService,
            IService<SignatoryEntity> signatoryService)
        {
            ((IManager<ProxyEntity>)this).Service = proxyService;
            ((IManager<SignatoryEntity>)this).Service = signatoryService;
        }
    }
}
