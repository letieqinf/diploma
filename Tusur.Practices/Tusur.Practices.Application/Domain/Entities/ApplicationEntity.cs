namespace Tusur.Practices.Application.Domain.Entities
{
    public class ApplicationEntity : DomainEntity
    {
        public Guid PracticeDateId { get; set; }
        public Guid StudentId { get; set; }
        public Guid? OrganizationId { get; set; }
        public ushort Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
