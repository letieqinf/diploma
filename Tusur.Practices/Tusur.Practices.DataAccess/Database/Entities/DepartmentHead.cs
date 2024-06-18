using System.ComponentModel.DataAnnotations;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Persistence.Database.Entities.Utils;

namespace Tusur.Practices.Persistence.Database.Entities
{
    public class DepartmentHead : Entity, IMappable<DepartmentHeadEntity, DepartmentHead>
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }

        public DateOnly IsHeadFrom { get; set; }
    }
}
