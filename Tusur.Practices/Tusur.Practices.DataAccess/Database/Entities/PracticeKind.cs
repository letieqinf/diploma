using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Persistence.Database.Entities.Utils;

namespace Tusur.Practices.Persistence.Database.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class PracticeKind : Entity, IMappable<PracticeKindEntity, PracticeKind>
    {
        public string Name { get; set; }
    }
}
