using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Domain.Models;
using Tusur.Practices.Application.Domain.Models.Result;
using Tusur.Practices.Application.Ports.Input;
using Tusur.Practices.Application.Ports.Input.Utils;
using Tusur.Practices.Application.Ports.Output;

namespace Tusur.Practices.Application.UseCases
{
    public class PropertyManager : IPropertyManager
    {
        private readonly IService<TeacherEntity> _teacherService;
        private readonly IService<StudentEntity> _studentService;
        private readonly IService<SupervisorEntity> _supervisorService;
        private readonly IService<ContractEntity> _contractService;
        private readonly IService<PracticeProfileEntity> _practiceProfileService;

        public PropertyManager(
            IService<TeacherEntity> teacherService,
            IService<StudentEntity> studentService,
            IService<SupervisorEntity> supervisorService,
            IService<ContractEntity> contractService,
            IService<PracticeProfileEntity> practiceProfileService)
        {
            _teacherService = teacherService;
            _studentService = studentService;
            _supervisorService = supervisorService;
            _contractService = contractService;
            _practiceProfileService = practiceProfileService;
        }

        public RequestResult IsInPropertyOf<TDomain>(Guid id, Guid userId, string role) where TDomain : DomainEntity
        {
            var type = typeof(TDomain);
            if (type != typeof(ContractEntity)
                && type != typeof(PracticeProfileEntity))
            {
                return new RequestResult
                {
                    Success = false,
                    Error = new ArgumentException().Message
                };
            }

            var result = ((IProperty<ContractEntity>)this).GetAllPropertyOf(userId, role);
            return new RequestResult { Success = result.Value!.FirstOrDefault(entity => entity.Id == id) != null, Error = result.Error };
        }

        RequestResult IProperty<ContractEntity>.IsInPropertyOf(Guid id, Guid userId, string role)
        {
            return IsInPropertyOf<ContractEntity>(id, userId, role);
        }

        RequestResult IProperty<PracticeProfileEntity>.IsInPropertyOf(Guid id, Guid userId, string role)
        {
            return IsInPropertyOf<PracticeProfileEntity>(id, userId, role);
        }

        public RequestResult<IEnumerable<TDomain>> GetAllPropertyOf<TDomain>(Guid userId, string role) where TDomain : DomainEntity
        {
            var type = typeof(TDomain);
            if (type != typeof(ContractEntity)
                && type != typeof(PracticeProfileEntity))
            {
                return new RequestResult<IEnumerable<TDomain>> 
                { 
                    Success = false, 
                    Error = new ArgumentException().Message 
                };
            }

            var result = ((IProperty<TDomain>)this).GetAllPropertyOf(userId, role);
            if (!result.Success)
                return new RequestResult<IEnumerable<TDomain>> { Success = false, Error = result.Error };

            var value = result.Value!.Cast<TDomain>();
            return new RequestResult<IEnumerable<TDomain>> { Success = true, Value = value };
        }

        RequestResult<IEnumerable<ContractEntity>> IProperty<ContractEntity>.GetAllPropertyOf(Guid userId, string role)
        {
            var result = new List<ContractEntity>();

            if (role == DomainDefaults.Teacher)
            {
                var teachers = _teacherService.GetBy(entity => entity.UserId == userId);
                if (!teachers.Success)
                    return new RequestResult<IEnumerable<ContractEntity>> { Success = false, Error = teachers.Error };

                foreach (var teacher in teachers.Value!)
                {
                    var supervisors = _supervisorService.GetBy(entity => entity.TeacherId == teacher.Id);
                    if (!supervisors.Success)
                        continue;

                    var dates = supervisors.Value!
                        .Select(entity => entity.PracticeDateId)
                        .Distinct();

                    foreach (var date in dates)
                    {
                        var profiles = _practiceProfileService.GetBy(entity => entity.PracticeDateId == date);
                        if (!profiles.Success)
                            continue;

                        var contracts = profiles.Value!
                            .Select(entity => entity.ContractId)
                            .Distinct();

                        foreach (var contract in contracts)
                        {
                            var value = _contractService.Find(contract);
                            if (!value.Success)
                                continue;

                            result.Add(value.Value!);
                        }
                    }
                }
            }

            if (role == DomainDefaults.Education)
            {
                var contracts = _contractService.GetAll();
                if (contracts.Success)
                    result.AddRange(contracts.Value!);
            }

            return new RequestResult<IEnumerable<ContractEntity>>
            {
                Success = true,
                Value = result
            };
        }

        RequestResult<IEnumerable<PracticeProfileEntity>> IProperty<PracticeProfileEntity>.GetAllPropertyOf(Guid userId, string role)
        {
            var result = new List<PracticeProfileEntity>();

            if (role == DomainDefaults.Teacher)
            {
                var teachers = _teacherService.GetBy(entity => entity.UserId == userId);
                if (!teachers.Success)
                    return new RequestResult<IEnumerable<PracticeProfileEntity>> { Success = false, Error = teachers.Error };

                var teacher = teachers.Value!.FirstOrDefault();
                if (teacher == null)
                    return new RequestResult<IEnumerable<PracticeProfileEntity>> { Success = false, Error = new ArgumentException().Message };

                var supervisors = _supervisorService.GetBy(entity => entity.TeacherId == teacher.Id);
                if (!supervisors.Success)
                    return new RequestResult<IEnumerable<PracticeProfileEntity>> { Success = false, Error = supervisors.Error };

                foreach (var supervisor in supervisors.Value!)
                {
                    var students = _studentService.GetBy(entity => entity.GroupId == supervisor.GroupId);
                    if (!students.Success)
                        continue;

                    foreach (var student in students.Value!)
                    {
                        var profiles = _practiceProfileService
                            .GetBy(entity => entity.PracticeDateId == supervisor.PracticeDateId && entity.StudentId == student.Id);

                        if (!profiles.Success)
                            continue;

                        result.AddRange(profiles.Value!);
                    }
                }
            }

            if (role == DomainDefaults.Education || role == DomainDefaults.Secretary)
            {
                var profiles = _practiceProfileService.GetAll();
                if (profiles.Success)
                    result.AddRange(profiles.Value!.Where(entity => entity.Status != 0));
            }

            return new RequestResult<IEnumerable<PracticeProfileEntity>>
            {
                Success = true,
                Value = result
            };
        }
    }
}
