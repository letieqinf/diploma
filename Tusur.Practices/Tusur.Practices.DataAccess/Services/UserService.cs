using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Domain.Models.Result;
using Tusur.Practices.Application.Ports.Output;
using Tusur.Practices.Persistence.Database.Entities;
using Tusur.Practices.Persistence.UnitsOfWork;

namespace Tusur.Practices.Persistence.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public UserService(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<RequestResult<IEnumerable<UserEntity>>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                return new RequestResult<IEnumerable<UserEntity>>
                {
                    Success = true,
                    Value = users.Select(entity => new UserEntity
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        LastName = entity.LastName,
                        Patronymic = entity.Patronymic,
                        Email = entity.Email!
                    })
                };
            }
            catch (Exception error)
            {
                return new RequestResult<IEnumerable<UserEntity>>
                {
                    Success = false,
                    Error = error.Message
                };
            }
        }

        public async Task<RequestResult<IEnumerable<UserEntity>>> GetUsersByAsync(Expression<Func<UserEntity, bool>> predicate)
        {
            try
            {
                var users = await _userManager.Users
                    .Select(entity => new UserEntity
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        LastName = entity.LastName,
                        Patronymic = entity.Patronymic,
                        Email = entity.Email!
                    }).AsQueryable().Where(predicate).ToListAsync();

                return new RequestResult<IEnumerable<UserEntity>>
                {
                    Success = users != null,
                    Value = users
                };
            }
            catch (Exception error)
            {
                return new RequestResult<IEnumerable<UserEntity>>
                {
                    Success = false,
                    Error = error.Message
                };
            }
        }

        public async Task<RequestResult<UserEntity>> FindUserAsync(Guid id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                return new RequestResult<UserEntity>
                {
                    Success = true,
                    Value = new UserEntity
                    {
                        Id = user!.Id,
                        Name = user.Name,
                        LastName = user.LastName,
                        Patronymic = user.Patronymic,
                        Email = user.Email!
                    }
                };
            }
            catch (Exception error)
            {
                return new RequestResult<UserEntity>
                {
                    Success = false,
                    Error = error.Message
                };
            }
        }

        public async Task<RequestResult<UserEntity>> FindUserAsync(Expression<Func<UserEntity, bool>> predicate)
        {
            try
            {
                var user = await _userManager.Users
                    .Select(entity => new UserEntity
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        LastName = entity.LastName,
                        Patronymic = entity.Patronymic,
                        Email = entity.Email!
                    }).FirstOrDefaultAsync(predicate);

                return new RequestResult<UserEntity>
                {
                    Success = user != null,
                    Value = user
                };
            }
            catch (Exception error)
            {
                return new RequestResult<UserEntity>
                {
                    Success = false,
                    Error = error.Message
                };
            }
        }

        public async Task<RequestResult<UserEntity>> CreateUserAsync(UserEntity entity, string password)
        {
            try
            {
                var user = new User
                {
                    Name = entity.Name,
                    LastName = entity.LastName,
                    Patronymic = entity.Patronymic,
                    UserName = entity.Email,
                    Email = entity.Email
                };

                await _userManager.CreateAsync(user, password);
                await _unitOfWork.SaveChangesAsync();

                user = await _userManager.FindByEmailAsync(entity.Email);
                entity.Id = user!.Id;

                return new RequestResult<UserEntity>
                {
                    Success = true,
                    Value = entity
                };
            }
            catch (Exception error)
            {
                return new RequestResult<UserEntity>
                {
                    Success = false,
                    Error = error.Message
                };
            }
        }

        public async Task<RequestResult<UserEntity>> UpdateUserAsync(UserEntity entity)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(entity.Id.ToString());

                user!.Name = entity.Name;
                user.LastName = entity.LastName;
                user.Patronymic = entity.Patronymic;

                if (user.Email != entity.Email)
                {
                    user.Email = user.UserName = entity.Email;
                    user.NormalizedEmail = user.NormalizedEmail = entity.Email.Normalize().ToUpper();
                }

                await _userManager.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();

                return new RequestResult<UserEntity>
                {
                    Success = true,
                    Value = entity
                };
            }
            catch (Exception error)
            {
                return new RequestResult<UserEntity>
                {
                    Success = false,
                    Error = error.Message
                };
            }
        }

        public async Task<RequestResult<UserEntity>> RemoveUserAsync(Guid id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                await _userManager.DeleteAsync(user!);
                await _unitOfWork.SaveChangesAsync();

                return new RequestResult<UserEntity>
                {
                    Success = true,
                    Value = new UserEntity
                    {
                        Id = user!.Id,
                        Name = user.Name,
                        LastName = user.LastName,
                        Patronymic = user.Patronymic,
                        Email = user.Email!
                    }
                };
            }
            catch (Exception error)
            {
                return new RequestResult<UserEntity>
                {
                    Success = false,
                    Error = error.Message
                };
            }
        }

        public async Task<RequestResult<UserEntity>> ValidateUserAsync(string email, string password)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                var result = await _userManager.CheckPasswordAsync(user, password);

                return new RequestResult<UserEntity>
                {
                    Success = result,
                    Value = result ? new UserEntity
                    {
                        Id = user!.Id,
                        Name = user.Name,
                        LastName = user.LastName,
                        Patronymic = user.Patronymic,
                        Email = user.Email!
                    } : null,
                    Error = result ? null : string.Empty
                };
            }
            catch (Exception error)
            {
                return new RequestResult<UserEntity>
                {
                    Success = false,
                    Error = error.Message
                };
            }
        }

        public RequestResult<RefreshTokenEntity> SetRefreshToken(Guid userId, RefreshTokenEntity entity)
        {
            try
            {
                var refreshTokenRepository = _unitOfWork.GetRepository<RefreshToken>();

                var refreshToken = refreshTokenRepository
                    .Find(entity => entity.UserId == userId)
                    .FirstOrDefault();

                if (refreshToken != null)
                {
                    refreshToken.Token = entity.Token;
                    refreshToken.ExpiresAt = entity.ExpiresAt;

                    refreshTokenRepository.Update(refreshToken);
                    _unitOfWork.SaveChanges();
                }
                else
                {
                    refreshToken = new RefreshToken
                    {
                        UserId = userId,
                        Token = entity.Token,
                        ExpiresAt = entity.ExpiresAt
                    };

                    refreshTokenRepository.Create(refreshToken);
                    _unitOfWork.SaveChanges();
                }

                entity.Id = refreshToken.Id;

                return new RequestResult<RefreshTokenEntity>
                {
                    Success = true,
                    Value = entity
                };
            }
            catch (Exception error)
            {
                return new RequestResult<RefreshTokenEntity>
                {
                    Success = false,
                    Error = error.Message
                };
            }
        }

        public RequestResult ValidateRefreshToken(Guid userId, string token)
        {
            try
            {
                var refreshTokenRepository = _unitOfWork.GetRepository<RefreshToken>();

                var refreshToken = refreshTokenRepository
                    .Find(entity => entity.UserId == userId)
                    .FirstOrDefault();

                if (refreshToken == null)
                    return new RequestResult { Success = false };

                return new RequestResult
                {
                    Success = refreshToken!.Token == token && refreshToken!.ExpiresAt > DateTime.UtcNow
                };
            }
            catch (Exception error)
            {
                return new RequestResult<RefreshTokenEntity>
                {
                    Success = false,
                    Error = error.Message
                };
            }
        }

        public RequestResult<RefreshTokenEntity> RemoveRefreshToken(Guid userId)
        {
            try
            {
                var refreshTokenRepository = _unitOfWork.GetRepository<RefreshToken>();

                var refreshToken = refreshTokenRepository
                    .Find(entity => entity.UserId == userId)
                    .FirstOrDefault();

                if (refreshToken == null)
                    return new RequestResult<RefreshTokenEntity> { Success = true };

                refreshToken = refreshTokenRepository.Remove(refreshToken);
                _unitOfWork.SaveChanges();

                return new RequestResult<RefreshTokenEntity>
                {
                    Success = true,
                    Value = new RefreshTokenEntity
                    {
                        Id = refreshToken.Id,
                        Token = refreshToken.Token,
                        ExpiresAt = refreshToken.ExpiresAt
                    }
                };
            }
            catch (Exception error)
            {
                return new RequestResult<RefreshTokenEntity>
                {
                    Success = false,
                    Error = error.Message
                };
            }
        }

        public async Task<RequestResult<IEnumerable<string>>> GetUserRolesAsync(Guid id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                var roles = await _userManager.GetRolesAsync(user!);

                return new RequestResult<IEnumerable<string>>()
                {
                    Success = true,
                    Value = roles.ToList()
                };
            }
            catch (Exception error)
            {
                return new RequestResult<IEnumerable<string>>
                {
                    Success = false,
                    Error = error.Message
                };
            }
        }

        public async Task<RequestResult> AddUserToRoleAsync(Guid id, string roleName)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                await _userManager.AddToRoleAsync(user!, roleName);
                await _unitOfWork.SaveChangesAsync();

                return new RequestResult
                {
                    Success = true
                };
            }
            catch (Exception error)
            {
                return new RequestResult
                {
                    Success = false,
                    Error = error.Message
                };
            }
        }

        public async Task<RequestResult> IsUserInRoleAsync(Guid userId, string roleName)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                return new RequestResult { Success = await _userManager.IsInRoleAsync(user!, roleName) };
            }
            catch (Exception error)
            {
                return new RequestResult
                {
                    Success = false,
                    Error = error.Message
                };
            }
        }
    }
}
