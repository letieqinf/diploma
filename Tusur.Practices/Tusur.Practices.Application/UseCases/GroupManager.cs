using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Domain.Models.Result;
using Tusur.Practices.Application.Ports.Input;
using Tusur.Practices.Application.Ports.Output;

namespace Tusur.Practices.Application.UseCases
{
    public class GroupManager : IGroupManager
    {
        IService<GroupEntity> IManager<GroupEntity>.Service { get; set; }

        public GroupManager(IService<GroupEntity> groupService)
        {
            ((IManager<GroupEntity>)this).Service = groupService;
        }
    }
}
