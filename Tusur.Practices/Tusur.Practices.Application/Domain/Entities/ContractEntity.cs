namespace Tusur.Practices.Application.Domain.Entities
{
    public class ContractEntity : DomainEntity
    {
        public Guid OrganizationId { get; set; }
        public Guid ContractContentId { get; set; }
        public bool IsLongTerm { get; set; }
    }
}
