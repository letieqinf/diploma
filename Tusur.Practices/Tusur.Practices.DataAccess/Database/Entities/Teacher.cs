using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Persistence.Database.Entities.Utils;

namespace Tusur.Practices.Persistence.Database.Entities
{
    [Index(nameof(UserId), nameof(DepartmentId), IsUnique = true)]
    public class Teacher : Entity, IMappable<TeacherEntity, Teacher>
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
