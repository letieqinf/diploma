namespace Tusur.Practices.Application.Domain.Entities
{
    public class StudyFieldEntity : DomainEntity
    {
        public Guid SpecialtyId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
