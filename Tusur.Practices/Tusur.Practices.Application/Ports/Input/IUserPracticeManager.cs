using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Domain.Models.Result;

namespace Tusur.Practices.Application.Ports.Input
{
    public interface IUserPracticeManager
    {
        public RequestResult<IEnumerable<PracticeDateEntity>> GetAllUserDates(Guid userId, string role);
        public RequestResult<IEnumerable<ApplicationEntity>> GetAllUserApplications(Guid userId, string role);
        public RequestResult<IEnumerable<SupervisorEntity>> GetAllUserSupervisors(Guid userId, string role);
    }
}
