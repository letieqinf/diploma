using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Domain.Models.Result;
using Tusur.Practices.Application.Ports.Input;
using Tusur.Practices.Application.Ports.Output;

namespace Tusur.Practices.Application.UseCases
{
    public class AuthManager : IAuthManager
    {
        private readonly IUserService _userService;

        public AuthManager(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<RequestResult<UserEntity>> CreateUserAsync(UserEntity entity, string password)
        {
            return await _userService.CreateUserAsync(entity, password);
        }

        public async Task<RequestResult<UserEntity>> ValidateUserAsync(string email, string password)
        {
            return await _userService.ValidateUserAsync(email, password);
        }

        public RequestResult SetRefreshToken(Guid userId, RefreshTokenEntity refreshTokenEntity)
        {
            return _userService.SetRefreshToken(userId, refreshTokenEntity);
        }

        public RequestResult ValidateRefreshToken(Guid userId, string token)
        {
            return _userService.ValidateRefreshToken(userId, token);
        }

        public RequestResult RemoveRefreshToken(Guid userId)
        {
            return _userService.RemoveRefreshToken(userId);
        }
    }
}
