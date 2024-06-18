using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Domain.Models.Result;

namespace Tusur.Practices.Application.Ports.Input
{
    public interface IAuthManager
    {
        public Task<RequestResult<UserEntity>> CreateUserAsync(UserEntity entity, string password);
        public Task<RequestResult<UserEntity>> ValidateUserAsync(string email, string password);

        public RequestResult SetRefreshToken(Guid userId, RefreshTokenEntity refreshTokenEntity);
        public RequestResult ValidateRefreshToken(Guid userId, string token);
        public RequestResult RemoveRefreshToken(Guid userId);
    }
}
