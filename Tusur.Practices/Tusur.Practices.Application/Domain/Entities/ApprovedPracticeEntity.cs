namespace Tusur.Practices.Application.Domain.Entities
{
    public class ApprovedPracticeEntity : DomainEntity
    {
        public Guid PracticeId { get; set; }
        public Guid StudyPlanId { get; set; }
    }
}
