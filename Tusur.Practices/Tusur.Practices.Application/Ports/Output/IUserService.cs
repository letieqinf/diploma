using System.Linq.Expressions;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Domain.Models.Result;

namespace Tusur.Practices.Application.Ports.Output
{
    public interface IUserService
    {
        public Task<RequestResult<IEnumerable<UserEntity>>> GetAllUsersAsync();
        public Task<RequestResult<IEnumerable<UserEntity>>> GetUsersByAsync(Expression<Func<UserEntity, bool>> predicate);
        public Task<RequestResult<UserEntity>> FindUserAsync(Guid id);
        public Task<RequestResult<UserEntity>> FindUserAsync(Expression<Func<UserEntity, bool>> predicate);
        public Task<RequestResult<UserEntity>> CreateUserAsync(UserEntity user, string password);
        public Task<RequestResult<UserEntity>> UpdateUserAsync(UserEntity user);
        public Task<RequestResult<UserEntity>> RemoveUserAsync(Guid id);

        public Task<RequestResult<UserEntity>> ValidateUserAsync(string email, string password);

        public RequestResult<RefreshTokenEntity> SetRefreshToken(Guid userId, RefreshTokenEntity entity);
        public RequestResult ValidateRefreshToken(Guid userId, string token);
        public RequestResult<RefreshTokenEntity> RemoveRefreshToken(Guid userId);

        public Task<RequestResult<IEnumerable<string>>> GetUserRolesAsync(Guid userId);
        public Task<RequestResult> AddUserToRoleAsync(Guid userId, string roleName);
        public Task<RequestResult> IsUserInRoleAsync(Guid userId, string roleName);
    }
}
