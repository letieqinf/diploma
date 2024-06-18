namespace Tusur.Practices.Application.Domain.Entities
{
    public class GroupEntity : DomainEntity
    {
        public Guid DepartmentId { get; set; }
        public Guid ApprovedStudyPlanId { get; set; }
        public string Name { get; set; }
    }
}
