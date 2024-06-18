using Tusur.Practices.Application.Domain.Entities;

namespace Tusur.Practices.Application.Ports.Output
{
    public interface ITokenService
    {
        public string CreateToken(UserEntity user, IEnumerable<string> roles);
        public RefreshTokenEntity CreateRefreshToken();
        public Guid GetId(string token);
    }
}
