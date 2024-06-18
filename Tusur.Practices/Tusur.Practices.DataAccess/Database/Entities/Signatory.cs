using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Persistence.Database.Entities.Utils;

namespace Tusur.Practices.Persistence.Database.Entities
{
    public class Signatory : Entity, IMappable<SignatoryEntity, Signatory>
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid ProxyId { get; set; }
        public Proxy Proxy { get; set; }
    }
}
