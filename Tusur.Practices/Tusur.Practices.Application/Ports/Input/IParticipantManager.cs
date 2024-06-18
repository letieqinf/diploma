using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Domain.Models.Result;

namespace Tusur.Practices.Application.Ports.Input
{
    public interface IParticipantManager : IManager<StudentEntity>, IManager<TeacherEntity>, IManager<DepartmentHeadEntity>, IManager<SupervisorEntity>
    {
        public RequestResult<TDomain> GetLastestByDepartmentId<TDomain>(Guid departmentId) where TDomain : DepartmentHeadEntity;
        public RequestResult<TDomain> GetLastestByUserId<TDomain>(Guid userId) where TDomain : DomainEntity;
    }
}
