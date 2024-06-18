using System.ComponentModel.DataAnnotations;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Persistence.Database.Entities.Utils;

namespace Tusur.Practices.Persistence.Database.Entities
{
    public class StudyPlan : Entity, IMappable<StudyPlanEntity, StudyPlan>
    {
        public Guid StudyFieldId { get; set; }
        public StudyField StudyField { get; set; }

        public Guid StudyFormId { get; set; }
        public StudyForm StudyForm { get; set; }

        public Guid DegreeId { get; set; }
        public Degree Degree { get; set; }
    }
}
