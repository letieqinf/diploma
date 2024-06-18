using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Persistence.Database.Entities.Utils;

namespace Tusur.Practices.Persistence.Database.Entities
{
    [Index(nameof(StudentId), nameof(PracticeDateId), IsUnique = true)]
    public class PracticeProfile : Entity, IMappable<PracticeProfileEntity, PracticeProfile>
    {
        public Guid StudentId { get; set; }
        public Student Student { get; set; }

        public Guid PracticeDateId { get; set; }
        public PracticeDate PracticeDate { get; set; }

        public Guid ContractId { get; set; }
        public Contract Contract { get; set; }

        public ushort Status { get; set; }
    }
}
