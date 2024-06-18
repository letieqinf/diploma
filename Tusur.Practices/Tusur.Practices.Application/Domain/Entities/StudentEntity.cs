namespace Tusur.Practices.Application.Domain.Entities
{
    public class StudentEntity : DomainEntity
    {
        public Guid UserId { get; set; }
        public Guid GroupId { get; set; }
        public DateOnly IsStudentFrom { get; set; }
    }
}
