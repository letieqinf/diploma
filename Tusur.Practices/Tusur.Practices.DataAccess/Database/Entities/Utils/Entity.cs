using System.ComponentModel.DataAnnotations;

namespace Tusur.Practices.Persistence.Database.Entities.Utils
{
    public class Entity : IEntity
    {
        [Key] public Guid Id { get; set; }

        public void Dispose() { }
    }
}
