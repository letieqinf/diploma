namespace Tusur.Practices.Application.Domain.Entities
{
    public class SignatoryEntity : DomainEntity
    {
        public Guid UserId { get; set; }
        public Guid ProxyId { get; set; }
    }
}
