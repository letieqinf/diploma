using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Persistence.Database.Entities.Utils;

namespace Tusur.Practices.Persistence.Database.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class StudyForm : Entity, IMappable<StudyFormEntity, StudyForm>
    {
        public string Name { get; set; }
    }
}
