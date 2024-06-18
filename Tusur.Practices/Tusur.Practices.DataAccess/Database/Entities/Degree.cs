using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Persistence.Database.Entities.Utils;

namespace Tusur.Practices.Persistence.Database.Entities
{
    public class Degree : Entity, IMappable<DegreeEntity, Degree>
    {
        public string Name { get; set; }
    }
}
