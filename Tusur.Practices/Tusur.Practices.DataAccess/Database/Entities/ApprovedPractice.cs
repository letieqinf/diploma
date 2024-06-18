using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Persistence.Database.Entities.Utils;

namespace Tusur.Practices.Persistence.Database.Entities
{
    public class ApprovedPractice : Entity, IMappable<ApprovedPracticeEntity, ApprovedPractice>
    {
        public Guid PracticeId { get; set; }
        public Practice Practice { get; set; }

        public Guid StudyPlanId { get; set; }
        public StudyPlan StudyPlan { get; set; }
    }
}
