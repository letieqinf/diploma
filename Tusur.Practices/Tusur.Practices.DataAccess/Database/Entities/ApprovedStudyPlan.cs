using Microsoft.EntityFrameworkCore;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Persistence.Database.Entities.Utils;

namespace Tusur.Practices.Persistence.Database.Entities
{
    [Index(nameof(Year), nameof(StudyPlanId), IsUnique = true)]
    public class ApprovedStudyPlan : Entity, IMappable<ApprovedStudyPlanEntity, ApprovedStudyPlan>
    {
        public ushort Year { get; set; }

        public Guid StudyPlanId { get; set; }
        public StudyPlan StudyPlan { get; set; }
    }
}
