namespace Tusur.Practices.Application.Domain.Entities
{
    public class DepartmentEntity : DomainEntity
    {
        public Guid FacultyId { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
    }
}
