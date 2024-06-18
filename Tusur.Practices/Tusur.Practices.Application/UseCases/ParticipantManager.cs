using System.Linq.Expressions;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Domain.Models.Result;
using Tusur.Practices.Application.Ports.Input;
using Tusur.Practices.Application.Ports.Output;

namespace Tusur.Practices.Application.UseCases
{
    public class ParticipantManager : IParticipantManager
    {
        IService<StudentEntity> IManager<StudentEntity>.Service { get; set; }
        IService<TeacherEntity> IManager<TeacherEntity>.Service { get; set; }
        IService<DepartmentHeadEntity> IManager<DepartmentHeadEntity>.Service { get; set; }
        IService<SupervisorEntity> IManager<SupervisorEntity>.Service { get; set; }

        public ParticipantManager(
            IService<StudentEntity> studentService,
            IService<TeacherEntity> teacherService,
            IService<DepartmentHeadEntity> departmentHeadService,
            IService<SupervisorEntity> supervisorService)
        {
            ((IManager<StudentEntity>)this).Service = studentService;
            ((IManager<TeacherEntity>)this).Service = teacherService;
            ((IManager<DepartmentHeadEntity>)this).Service = departmentHeadService;
            ((IManager<SupervisorEntity>)this).Service = supervisorService;
        }

        public RequestResult<TDomain> GetLastestByDepartmentId<TDomain>(Guid departmentId) where TDomain : DepartmentHeadEntity
        {
            var heads = ((IManager<DepartmentHeadEntity>)this).Service.GetBy(entity => entity.DepartmentId == departmentId);
            if (!heads.Success)
                return new RequestResult<TDomain> { Success = false, Error = heads.Error };

            return new RequestResult<TDomain>
            {
                Success = true,
                Value = (TDomain)heads.Value!.MaxBy(entity => ((TDomain)entity).IsHeadFrom)!
            };
        }

        public RequestResult<TDomain> GetLastestByUserId<TDomain>(Guid userId) where TDomain : DomainEntity
        {
            var type = typeof(TDomain);
            if (type != typeof(StudentEntity) && type != typeof(DepartmentHeadEntity))
                return new RequestResult<TDomain> { Success = false, Error = new ArgumentException().Message };

            if (type == typeof(DepartmentHeadEntity))
            {
                var heads = ((IManager<DepartmentHeadEntity>)this).Service.GetBy(entity => entity.UserId == userId);
                if (!heads.Success)
                    return new RequestResult<TDomain> { Success = false, Error = heads.Error };

                return new RequestResult<TDomain>
                {
                    Success = true,
                    Value = (TDomain)(DomainEntity)heads.Value!.MaxBy(entity => entity.IsHeadFrom)!
                };
            }

            var students = ((IManager<StudentEntity>)this).Service.GetBy(entity => entity.UserId == userId);
            if (!students.Success)
                return new RequestResult<TDomain> { Success = false, Error = students.Error };

            return new RequestResult<TDomain>
            {
                Success = true,
                Value = (TDomain)(DomainEntity)students.Value!.MaxBy(entity => entity.IsStudentFrom)!
            };
        }
    }
}
