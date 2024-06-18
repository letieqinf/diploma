namespace Tusur.Practices.Application.Domain.Entities
{
    public class PracticeProfileEntity : DomainEntity
    {
        public Guid PracticeDateId { get; set; }
        public Guid ContractId { get; set; }
        public Guid StudentId { get; set; }
        public ushort Status { get; set; }
    }
}
