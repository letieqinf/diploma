using Microsoft.EntityFrameworkCore;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Persistence.Database.Entities.Utils;

namespace Tusur.Practices.Persistence.Database.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class Specialty : Entity, IMappable<SpecialtyEntity, Specialty>
    {
        public string Name { get; set; }
        public Guid FacultyId { get; set; }
        public Faculty FactultyId { get; set; }
    }
}
