namespace Tusur.Practices.Application.Domain.Entities
{
    public class SpecialtyEntity : DomainEntity
    {
        public Guid FacultyId { get; set; }
        public string Name { get; set; }
    }
}
