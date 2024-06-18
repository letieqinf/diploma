namespace Tusur.Practices.Application.Domain.Entities
{
    public class UserEntity : DomainEntity
    {
        public string Email { get; set; }

        public string Name { get; set; }
        public string LastName { get; set; }
        public string? Patronymic { get; set; }
    }
}
