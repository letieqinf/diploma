using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tusur.Practices.Application.Domain.Entities;

namespace Tusur.Practices.Jwt.Extensions
{
    internal static class JwtBearerExtension
    {
        public static IEnumerable<Claim> CreateClaims(this UserEntity user, IEnumerable<string>? roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            if (roles is not null)
                foreach (var role in roles)
                    claims.Add(new Claim(ClaimTypes.Role, role));

            return claims;
        }

        public static JwtSecurityToken CreateJwtToken(this IEnumerable<Claim> claims)
        {
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(Defaults.JwtBearerDefaults.TokenSecret)
                ),
                SecurityAlgorithms.HmacSha256
            );

            return new JwtSecurityToken(
                issuer: Defaults.JwtBearerDefaults.Issuer,
                audience: Defaults.JwtBearerDefaults.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Defaults.JwtBearerDefaults.TokenExpiresInMinutes),
                signingCredentials: signingCredentials
            );
        }
    }
}
