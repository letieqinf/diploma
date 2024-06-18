using System.ComponentModel.DataAnnotations;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Persistence.Database.Entities.Utils;

namespace Tusur.Practices.Persistence.Database.Entities
{
    public class Proxy : Entity, IMappable<ProxyEntity, Proxy>
    {
        public DateOnly ValidFrom { get; set; }
    }
}
