using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Domain.Models;
using Tusur.Practices.Application.Domain.Models.Result;
using Tusur.Practices.Application.Ports.Input;
using Tusur.Practices.Application.Ports.Output;

namespace Tusur.Practices.Application.UseCases
{
    public class UserPracticeManager : IUserPracticeManager
    {
        private readonly IService<ApplicationEntity> _applicationService;
        private readonly IService<PracticeDateEntity> _practiceDateService;
        private readonly IService<StudentEntity> _studentService;
        private readonly IService<SupervisorEntity> _supervisorService;
        private readonly IService<TeacherEntity> _teacherService;
        private readonly IService<GroupEntity> _groupService;

        public UserPracticeManager(
            IService<ApplicationEntity> applicationService,
            IService<PracticeDateEntity> practiceDateService,
            IService<StudentEntity> studentService,
            IService<SupervisorEntity> supervisorService,
            IService<TeacherEntity> teacherService,
            IService<GroupEntity> groupService)
        {
            _applicationService = applicationService;
            _practiceDateService = practiceDateService;
            _studentService = studentService;
            _groupService = groupService;
            _supervisorService = supervisorService;
            _teacherService = teacherService;
        }

        public RequestResult<IEnumerable<PracticeDateEntity>> GetAllUserDates(Guid userId, string role)
        {
            var dates = new List<PracticeDateEntity>();

            if (role == DomainDefaults.Student)
            {
                var students = _studentService.GetBy(entity => entity.Id == userId);
                if (!students.Success)
                    return new RequestResult<IEnumerable<PracticeDateEntity>> { Success = false, Error = students.Error };

                foreach (var student in students.Value!)
                {
                    var group = _groupService.Find(student.GroupId);
                    if (!group.Success)
                        continue;

                    var practiceDates = _practiceDateService.GetBy(entity => entity.ApprovedStudyPlanId == group.Value!.ApprovedStudyPlanId);
                    if (!practiceDates.Success)
                        continue;

                    dates.AddRange(practiceDates.Value!);
                }
            }

            if (role == DomainDefaults.Education)
            {
                var result = _practiceDateService.GetAll();
                if (result.Success)
                    dates.AddRange(result.Value!);
            }

            return new RequestResult<IEnumerable<PracticeDateEntity>>
            {
                Success = true,
                Value = dates
            };
        }

        public RequestResult<IEnumerable<ApplicationEntity>> GetAllUserApplications(Guid userId, string role)
        {
            var applications = new List<ApplicationEntity>();

            if (role == DomainDefaults.Student)
            {
                var students = _studentService.GetBy(entity => entity.Id == userId);
                if (!students.Success)
                    return new RequestResult<IEnumerable<ApplicationEntity>> { Success = false, Error = students.Error };

                foreach (var student in students.Value!)
                {
                    var group = _groupService.Find(student.GroupId);
                    if (!group.Success)
                        continue;

                    var practiceDates = _practiceDateService.GetBy(entity => entity.ApprovedStudyPlanId == group.Value!.ApprovedStudyPlanId);
                    if (!practiceDates.Success)
                        continue;

                    foreach (var date in practiceDates.Value!)
                    {
                        var application = _applicationService.Find(entity => entity.PracticeDateId == date.Id && entity.StudentId == student.Id);
                        if (!application.Success)
                            continue;

                        if (application.Value == null)
                            continue;

                        applications.Add(application.Value!);
                    }
                }
            }

            return new RequestResult<IEnumerable<ApplicationEntity>>
            {
                Success = true,
                Value = applications
            };
        }

        public RequestResult<IEnumerable<SupervisorEntity>> GetAllUserSupervisors(Guid userId, string role)
        {
            var supervisors = new List<SupervisorEntity>();

            if (role == DomainDefaults.Student)
            {
                var students = _studentService.GetBy(entity => entity.Id == userId);
                if (!students.Success)
                    return new RequestResult<IEnumerable<SupervisorEntity>> { Success = false, Error = students.Error };

                foreach (var student in students.Value!)
                {
                    var group = _groupService.Find(student.GroupId);
                    if (!group.Success)
                        continue;

                    var result = _supervisorService.GetBy(entity => entity.GroupId == group.Value!.Id);
                    if (!result.Success)
                        continue;

                    supervisors.AddRange(result.Value!);
                }
            }
            else if (role == DomainDefaults.Teacher)
            {
                var teachers = _teacherService.GetBy(entity => entity.UserId == userId);
                if (!teachers.Success)
                    return new RequestResult<IEnumerable<SupervisorEntity>> { Success = false, Error = teachers.Error };

                foreach (var teacher in teachers.Value!)
                {
                    var result = _supervisorService.GetBy(entity => entity.TeacherId == teacher.Id);
                    if (!result.Success)
                        continue;

                    supervisors.AddRange(result.Value!);
                }
            }
            else if (role == DomainDefaults.Education)
            {
                var result = _supervisorService.GetAll();
                if (result.Success)
                    supervisors.AddRange(result.Value!);
            }

            return new RequestResult<IEnumerable<SupervisorEntity>>
            {
                Success = true,
                Value = supervisors
            };
        }

        public RequestResult<IEnumerable<SupervisorEntity>> GetSupervisorsByPracticeDateId(Guid practiceDateId)
        {
            return _supervisorService.GetBy(entity => entity.PracticeDateId == practiceDateId);
        }
    }
}
