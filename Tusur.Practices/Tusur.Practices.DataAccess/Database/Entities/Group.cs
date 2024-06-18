using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Persistence.Database.Entities.Utils;

namespace Tusur.Practices.Persistence.Database.Entities
{
    [Index(nameof(ApprovedStudyPlanId), nameof(Name), IsUnique = true)]
    public class Group : Entity, IMappable<GroupEntity, Group>
    {
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }

        public Guid ApprovedStudyPlanId { get; set; }
        public ApprovedStudyPlan ApprovedStudyPlan { get; set; }

        public string Name { get; set; }
    }
}
