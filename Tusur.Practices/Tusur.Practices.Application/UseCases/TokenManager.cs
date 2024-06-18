using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Ports.Input;
using Tusur.Practices.Application.Ports.Output;

namespace Tusur.Practices.Application.UseCases
{
    public class TokenManager : ITokenManager
    {
        private readonly ITokenService _tokenService;

        public TokenManager(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public string CreateToken(UserEntity user, IEnumerable<string> roles)
        {
            return _tokenService.CreateToken(user, roles);
        }

        public RefreshTokenEntity CreateRefreshToken()
        {
            return _tokenService.CreateRefreshToken();
        }

        public Guid GetId(string token)
        {
            return _tokenService.GetId(token);
        }
    }
}
