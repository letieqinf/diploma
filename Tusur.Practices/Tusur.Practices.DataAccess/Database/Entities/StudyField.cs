using Microsoft.EntityFrameworkCore;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Persistence.Database.Entities.Utils;

namespace Tusur.Practices.Persistence.Database.Entities
{
    [Index(nameof(Code), nameof(SpecialtyId), IsUnique = true)]
    public class StudyField : Entity, IMappable<StudyFieldEntity, StudyField>
    {
        public Guid SpecialtyId { get; set; }
        public Specialty Specialty { get; set; }

        public string Name { get; set; }
        public string Code { get; set; }
    }
}
