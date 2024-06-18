using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Ports.Input;
using Tusur.Practices.Application.Ports.Output;

namespace Tusur.Practices.Application.UseCases
{
    public class ApplicationManager : IApplicationManager
    {
        IService<ApplicationEntity> IManager<ApplicationEntity>.Service { get; set; }
        IService<CommentEntity> IManager<CommentEntity>.Service { get; set; }

        public ApplicationManager(IService<ApplicationEntity> applicationService, IService<CommentEntity> commentService)
        {
            ((IManager<ApplicationEntity>)this).Service = applicationService;
            ((IManager<CommentEntity>)this).Service = commentService;
        }
    }
}
