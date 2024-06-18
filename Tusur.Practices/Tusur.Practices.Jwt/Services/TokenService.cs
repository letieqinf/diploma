using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Ports.Output;
using Tusur.Practices.Jwt.Extensions;

namespace Tusur.Practices.Jwt.Services
{
    public class TokenService : ITokenService
    {
        public string CreateToken(UserEntity user, IEnumerable<string> roles)
        {
            var token = user
                .CreateClaims(roles)
                .CreateJwtToken();

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public RefreshTokenEntity CreateRefreshToken()
        {
            var refreshToken = new RefreshTokenEntity
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)),
                ExpiresAt = DateTime.UtcNow.AddDays(Defaults.JwtBearerDefaults.RefreshTokenExpiresInDays)
            };

            return refreshToken;
        }

        public Guid GetId(string token)
        {
            var decode = new JwtSecurityToken(jwtEncodedString: token);
            var claims = decode.Claims;

            var id = claims
                .Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(c => c.Value)
                .FirstOrDefault();

            return Guid.Parse(id!);
        }
    }
}
