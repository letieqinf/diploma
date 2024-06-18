namespace Tusur.Practices.Application.Domain.Entities
{
    public class SupervisorEntity : DomainEntity
    {
        public Guid GroupId { get; set; }
        public Guid PracticeDateId { get; set; }
        public Guid TeacherId { get; set; }
        public bool IsHead { get; set; }
    }
}
