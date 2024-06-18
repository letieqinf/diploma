using Tusur.Practices.Application.Domain.Entities;

namespace Tusur.Practices.Application.Ports.Input
{
    public interface IProxyManager : IManager<ProxyEntity>, IManager<SignatoryEntity>
    {
    }
}
