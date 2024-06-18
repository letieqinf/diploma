using Tusur.Practices.Application.Domain.Entities;

namespace Tusur.Practices.Application.Ports.Input
{
    public interface IApplicationManager : IManager<ApplicationEntity>, IManager<CommentEntity>
    {

    }
}
