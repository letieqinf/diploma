using System.ComponentModel.DataAnnotations;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Persistence.Database.Entities.Utils;

namespace Tusur.Practices.Persistence.Database.Entities
{
    public class Practice : Entity, IMappable<PracticeEntity, Practice>
    {
        public Guid PracticeTypeId { get; set; }
        public PracticeType PracticeType { get; set; }

        public Guid PracticeKindId { get; set; }
        public PracticeKind PracticeKind { get; set; }
    }
}
