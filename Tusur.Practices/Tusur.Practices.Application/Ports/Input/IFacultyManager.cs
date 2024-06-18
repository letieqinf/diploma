using Tusur.Practices.Application.Domain.Entities;

namespace Tusur.Practices.Application.Ports.Input
{
    public interface IFacultyManager : IManager<FacultyEntity>, IManager<DepartmentEntity>
    {

    }
}
