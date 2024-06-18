namespace Tusur.Practices.Application.Domain.Entities
{
    public class PracticeDateEntity : DomainEntity
    {
        public Guid ApprovedPracticeId { get; set; }
        public Guid ApprovedStudyPlanId { get; set; }
        public DateOnly StartsAt { get; set; }
        public DateOnly EndsAt { get; set; }
    }
}
