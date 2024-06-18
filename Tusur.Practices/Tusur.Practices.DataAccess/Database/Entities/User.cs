using Microsoft.AspNetCore.Identity;

namespace Tusur.Practices.Persistence.Database.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string? Patronymic { get; set; }
    }
}
