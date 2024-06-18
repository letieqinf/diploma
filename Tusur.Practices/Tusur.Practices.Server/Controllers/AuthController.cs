using Microsoft.AspNetCore.Mvc;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Ports.Input;
using Tusur.Practices.Server.Models.Request;
using Tusur.Practices.Server.Models.Response;

namespace Tusur.Practices.Server.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthManager _authManager;
        private readonly IUserAccountManager _userAccountManager;
        private readonly ITokenManager _tokenManager;

        public AuthController(IAuthManager authManager, IUserAccountManager userAccountManager, ITokenManager tokenManager)
        {
            _authManager = authManager;
            _userAccountManager = userAccountManager;
            _tokenManager = tokenManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<LoginResponseModel>> Login([FromBody] LoginRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var validResult = await _authManager.ValidateUserAsync(model.Email, model.Password);
            if (!validResult.Success)
                return Unauthorized();

            var rolesResult = await _userAccountManager.GetUserRolesAsync(validResult.Value!.Id);
            if (!rolesResult.Success)
                return Unauthorized();

            var jwtToken = _tokenManager.CreateToken(validResult.Value!, rolesResult.Value!);
            var refreshToken = _tokenManager.CreateRefreshToken();

            Response.Cookies.Append("refresh", refreshToken.Token, new CookieOptions
            {
                HttpOnly = true
            });

            Response.Cookies.Append("uid", validResult.Value!.Id.ToString(), new CookieOptions
            {
                HttpOnly = true
            });

            _authManager.SetRefreshToken(validResult.Value!.Id, refreshToken);

            return Ok(new LoginResponseModel { AccessToken = jwtToken, Roles = rolesResult.Value! });
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var regResult = await _authManager.CreateUserAsync(new UserEntity
            {
                Email = model.Email,
                Name = model.Name,
                LastName = model.LastName,
                Patronymic = model.Patronymic
            }, model.Password);

            if (!regResult.Success)
                return BadRequest();

            await _userAccountManager.AddUserToRoleAsync(
                regResult.Value!.Id, 
                "user"
            );

            return Ok();
        }

        [HttpPost]
        [Route("logout")]
        public IActionResult Logout()
        {
            if (Request.Cookies["refresh"] is null)
                return NoContent();

            var userId = Guid.Parse(Request.Cookies["uid"]!);

            _authManager.RemoveRefreshToken(userId);
            Response.Cookies.Delete("refresh");
            Response.Cookies.Delete("uid");

            return Ok();
        }

        [HttpGet]
        [Route("refresh")]
        public async Task<ActionResult<RefreshResponseModel>> Refresh()
        {
            var refreshString = Request.Cookies["refresh"];

            if (refreshString is null || Request.Cookies["uid"] is null)
                return Unauthorized();

            var userId = Guid.Parse(Request.Cookies["uid"]!);

            var isValidated = _authManager.ValidateRefreshToken(userId, refreshString);
            if (!isValidated.Success)
                return Unauthorized();

            var userResult = await _userAccountManager.FindUserAsync(userId);
            if (!userResult.Success)
                return Unauthorized();

            var rolesResult = await _userAccountManager.GetUserRolesAsync(userId);
            if (!rolesResult.Success)
                return Unauthorized();

            var newAccessToken = _tokenManager.CreateToken(userResult.Value!, rolesResult.Value!);

            var newRefreshToken = _tokenManager.CreateRefreshToken();
            _authManager.SetRefreshToken(userId, newRefreshToken);
            Response.Cookies.Append("refresh", newRefreshToken.Token);

            return Ok(new RefreshResponseModel { AccessToken = newAccessToken, Roles = rolesResult.Value! });
        }
    }
}
