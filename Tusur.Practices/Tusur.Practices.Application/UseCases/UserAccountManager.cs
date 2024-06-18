using System.Linq.Expressions;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Domain.Models.Result;
using Tusur.Practices.Application.Ports.Input;
using Tusur.Practices.Application.Ports.Output;

namespace Tusur.Practices.Application.UseCases
{
    public class UserAccountManager : IUserAccountManager
    {
        private readonly IUserService _userService;

        public UserAccountManager(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<RequestResult<IEnumerable<UserEntity>>> GetAllUsersAsync()
        {
            return await _userService.GetAllUsersAsync();
        }

        public async Task<RequestResult<IEnumerable<UserEntity>>> GetUsersByAsync(Expression<Func<UserEntity, bool>> predicate)
        {
            return await _userService.GetUsersByAsync(predicate);
        }

        public async Task<RequestResult<UserEntity>> FindUserAsync(Guid id)
        {
            return await _userService.FindUserAsync(id);
        }

        public async Task<RequestResult<UserEntity>> FindUserAsync(Expression<Func<UserEntity, bool>> predicate)
        {
            return await _userService.FindUserAsync(predicate);
        }

        public async Task<RequestResult<IEnumerable<string>>> GetUserRolesAsync(Guid userId)
        {
            return await _userService.GetUserRolesAsync(userId);
        }

        public async Task<RequestResult> AddUserToRoleAsync(Guid userId, string roleName)
        {
            return await _userService.AddUserToRoleAsync(userId, roleName);
        }

        public async Task<RequestResult> IsUserInRoleAsync(Guid userId, string role)
        {
            return await _userService.IsUserInRoleAsync(userId, role);
        }
    }
}
