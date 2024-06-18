using System.ComponentModel.DataAnnotations;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Persistence.Database.Entities.Utils;

namespace Tusur.Practices.Persistence.Database.Entities
{
    public class Application : Entity, IMappable<ApplicationEntity, Application>
    {
        public Guid StudentId { get; set; }
        public Student Student { get; set; }

        public Guid PracticeDateId { get; set; }
        public PracticeDate PracticeDate { get; set; }

        public Guid? OrganizationId { get; set; }
        public Organization Organization { get; set; }

        public ushort Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
