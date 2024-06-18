using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Ports.Input;
using Tusur.Practices.Server.Models.Response;

namespace Tusur.Practices.Server.Controllers
{
    [Route("api/faculties")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FacultyController : ControllerBase
    {
        private readonly IParticipantManager _participantManager;
        private readonly IUserAccountManager _userAccountManager;
        private readonly IFacultyManager _facultyManager;

        public FacultyController(
            IParticipantManager participantManager,
            IUserAccountManager userAccountManager,
            IFacultyManager facultyManager)
        {
            _participantManager = participantManager;
            _userAccountManager = userAccountManager;
            _facultyManager = facultyManager;
        }

        [HttpGet]
        [Route("departments/teachers")]
        public async Task<IActionResult> GetTeachers()
        {
            var teachers = _participantManager.GetAll<TeacherEntity>();
            if (!teachers.Success)
                return BadRequest();

            var result = new List<GetTeachersResponseModel>();
            foreach (var teacher in teachers.Value!)
            {
                var user = await _userAccountManager.FindUserAsync(teacher.UserId);
                if (!user.Success)
                    continue;

                result.Add(new GetTeachersResponseModel
                {
                    Id = user.Value!.Id,
                    Name = user.Value!.Name,
                    LastName = user.Value!.LastName,
                    Patronymic = user.Value!.Patronymic,
                    Email = user.Value!.Email,
                    TeacherId = teacher.Id,
                    DepartmentId = teacher.DepartmentId
                });
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("departments/teachers/{id:guid}")]
        public async Task<IActionResult> GetTeacher([FromRoute] Guid id)
        {
            var teacher = _participantManager.Find<TeacherEntity>(id);
            if (!teacher.Success)
                return BadRequest();

            var user = await _userAccountManager.FindUserAsync(teacher.Value!.UserId);
            if (!user.Success)
                return BadRequest();

            return Ok(new GetTeachersResponseModel
            {
                Id = user.Value!.Id,
                Name = user.Value!.Name,
                LastName = user.Value!.LastName,
                Patronymic = user.Value!.Patronymic,
                Email = user.Value!.Email,
                TeacherId = teacher.Value!.Id,
                DepartmentId = teacher.Value!.DepartmentId
            });
        }
    }
}
