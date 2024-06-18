namespace Tusur.Practices.Application.Domain.Entities
{
    
    public class ContractContentEntity : DomainEntity
    {
        public string UniResp { get; set; }
        public string OrgResp { get; set; }
        public string UniCan { get; set; }
        public string OrgCan { get; set; }

        public bool? IsDefault { get; set; }
    }
}
