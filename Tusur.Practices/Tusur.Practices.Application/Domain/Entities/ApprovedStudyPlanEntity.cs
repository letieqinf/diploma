namespace Tusur.Practices.Application.Domain.Entities
{
    public class ApprovedStudyPlanEntity : DomainEntity
    {
        public Guid StudyPlanId { get; set; }
        public ushort Year { get; set; }
    }
}
