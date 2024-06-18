using System.ComponentModel.DataAnnotations;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Persistence.Database.Entities.Utils;

namespace Tusur.Practices.Persistence.Database.Entities
{
    public class PracticeType : Entity, IMappable<PracticeTypeEntity, PracticeType>
    {
        public string Name { get; set; }
    }
}
