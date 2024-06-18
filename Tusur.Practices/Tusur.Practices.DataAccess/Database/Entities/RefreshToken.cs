using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Persistence.Database.Entities.Utils;

namespace Tusur.Practices.Persistence.Database.Entities
{
    public class RefreshToken : Entity, IMappable<RefreshTokenEntity, RefreshToken>
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
