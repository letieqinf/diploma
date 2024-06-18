namespace Tusur.Practices.Application.Domain.Entities
{
    public class PracticeEntity : DomainEntity
    {
        public Guid PracticeKindId { get; set; }
        public Guid PracticeTypeId { get; set; }
    }
}
