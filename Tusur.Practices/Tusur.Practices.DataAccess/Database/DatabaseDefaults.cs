using Microsoft.AspNetCore.Identity;
using Tusur.Practices.Application.Domain.Models;

namespace Tusur.Practices.Persistence.Database
{
    public class DatabaseDefaults : DomainDefaults
    {
        public static IEnumerable<IdentityRole<Guid>> GetIdentityRoles()
        {
            var roles = DefaultRoles.Select(role => new IdentityRole<Guid>
            {
                Id = Guid.NewGuid(),
                Name = role,
                NormalizedName = role.Normalize().ToUpper()
            });
            return roles;
        }
    }
}
