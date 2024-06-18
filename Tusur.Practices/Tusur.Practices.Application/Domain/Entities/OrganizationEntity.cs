namespace Tusur.Practices.Application.Domain.Entities
{
    public class OrganizationEntity : DomainEntity
    {
        public long? Inn { get; set; }
        public long? Trrc { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationAddress { get; set; }
        public bool IsApproved { get; set; } = false;
    }
}
