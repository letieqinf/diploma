using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Persistence.Database.Entities.Utils;

namespace Tusur.Practices.Persistence.Database.Entities
{
    [Index(nameof(TeacherId), nameof(GroupId), nameof(PracticeDateId), IsUnique = true)]
    [Index(nameof(IsHead), IsUnique = true)]
    public class Supervisor : Entity, IMappable<SupervisorEntity, Supervisor>
    {
        public Guid TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public Guid GroupId { get; set; }
        public Group Group { get; set; }

        public Guid PracticeDateId { get; set; }
        public PracticeDate PracticeDate { get; set; }

        public bool? IsHead { get; set; } = null;
    }
}
