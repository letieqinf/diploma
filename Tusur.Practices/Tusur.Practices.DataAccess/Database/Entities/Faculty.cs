using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Persistence.Database.Entities.Utils;

namespace Tusur.Practices.Persistence.Database.Entities
{
    [Index(nameof(Name), IsUnique = true), Index(nameof(Abbreviation), IsUnique = true)]
    public class Faculty : Entity, IMappable<FacultyEntity, Faculty>
    {
        [Required] public string Name { get; set; }
        [Required] public string Abbreviation { get; set; }
    }
}
