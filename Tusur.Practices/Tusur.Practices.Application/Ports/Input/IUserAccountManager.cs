using System.Linq.Expressions;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Domain.Models.Result;

namespace Tusur.Practices.Application.Ports.Input
{
    public interface IUserAccountManager
    {
        public Task<RequestResult<IEnumerable<UserEntity>>> GetAllUsersAsync();
        public Task<RequestResult<IEnumerable<UserEntity>>> GetUsersByAsync(Expression<Func<UserEntity, bool>> predicate);
        public Task<RequestResult<UserEntity>> FindUserAsync(Guid id);
        public Task<RequestResult<UserEntity>> FindUserAsync(Expression<Func<UserEntity, bool>> predicate);

        public Task<RequestResult<IEnumerable<string>>> GetUserRolesAsync(Guid userId);
        public Task<RequestResult> AddUserToRoleAsync(Guid userId, string roleName);
        public Task<RequestResult> IsUserInRoleAsync(Guid userId, string role);
    }
}
