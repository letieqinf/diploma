using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Ports.Input;
using Tusur.Practices.Application.Ports.Output;

namespace Tusur.Practices.Application.UseCases
{
    public class FacultyManager : IFacultyManager
    {
        IService<DepartmentEntity> IManager<DepartmentEntity>.Service { get; set; }
        IService<FacultyEntity> IManager<FacultyEntity>.Service { get; set; }

        public FacultyManager(IService<DepartmentEntity> departmentService, IService<FacultyEntity> facultyService)
        {
            ((IManager<DepartmentEntity>)this).Service = departmentService;
            ((IManager<FacultyEntity>)this).Service = facultyService;
        }
    }
}
