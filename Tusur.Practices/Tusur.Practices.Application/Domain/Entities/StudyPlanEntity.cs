namespace Tusur.Practices.Application.Domain.Entities
{
    public class StudyPlanEntity : DomainEntity
    {
        public Guid DegreeId { get; set; }
        public Guid StudyFromId { get; set; }
        public Guid StudyFieldId { get; set; }
    }
}
