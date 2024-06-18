namespace Tusur.Practices.Application.Domain.Entities
{
    public class RefreshTokenEntity : DomainEntity
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
