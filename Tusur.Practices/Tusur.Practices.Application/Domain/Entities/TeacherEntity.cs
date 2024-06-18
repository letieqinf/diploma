namespace Tusur.Practices.Application.Domain.Entities
{
    public class TeacherEntity : DomainEntity
    {
        public Guid UserId { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
