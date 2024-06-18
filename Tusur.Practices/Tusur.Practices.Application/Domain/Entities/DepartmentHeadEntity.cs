namespace Tusur.Practices.Application.Domain.Entities
{
    public class DepartmentHeadEntity : DomainEntity
    {
        public Guid UserId { get; set; }
        public Guid DepartmentId { get; set; }
        public DateOnly IsHeadFrom { get; set; }
    }
}
