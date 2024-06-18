using Tusur.Practices.Application.Domain.Entities;

namespace Tusur.Practices.Application.Ports.Input
{
    public interface ITokenManager
    {
        public string CreateToken(UserEntity user, IEnumerable<string> roles);
        public RefreshTokenEntity CreateRefreshToken();
        public Guid GetId(string token);
    }
}
