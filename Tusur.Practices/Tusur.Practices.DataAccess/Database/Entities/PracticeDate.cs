using System.ComponentModel.DataAnnotations;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Persistence.Database.Entities.Utils;

namespace Tusur.Practices.Persistence.Database.Entities
{
    public class PracticeDate : Entity, IMappable<PracticeDateEntity, PracticeDate>
    {
        public Guid ApprovedStudyPlanId { get; set; }
        public ApprovedStudyPlan ApprovedStudyPlan { get; set; }

        public Guid ApprovedPracticeId { get; set; }
        public ApprovedPractice ApprovedPractice { get; set; }

        public DateOnly StartsAt { get; set; }
        public DateOnly EndsAt { get; set; }
    }
}
