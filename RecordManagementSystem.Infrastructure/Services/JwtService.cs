using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RecordManagementSystem.Application.Features.Account.Interface;
using RecordManagementSystem.Application.Features.Account.DTO;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace RecordManagementSystem.Infrastructure.Services
{
    public class JwtService : IJwtToken
    {
        private readonly IConfiguration _configuration;
        private readonly Byte[] _key;
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
            _key = Encoding.UTF8.GetBytes(_configuration["Jwt:key"]!);
        }


        public string GenerateAccessJwtToken(JwtApplicationUser user, IEnumerable<Claim>? additionalClaims = null)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var durationInMinutes = double.Parse(jwtSettings["DurationInMinutes"] ?? "10");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.username ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("uid", user.id.ToString()),
                new Claim("name", user.username ?? "")
            };

            //add roles
            claims.AddRange(user.Roles.Select(role => new Claim("role", role)));

            if(additionalClaims is not null) claims.AddRange(additionalClaims);

            var credentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256);
            var JwtToken = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(durationInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(JwtToken);
        }


        public string GenerateRefreshJwtToken()
        {
            var randomBytes = new byte[64];
            using var randomNumGenerator = RandomNumberGenerator.Create();
            randomNumGenerator.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        public string HashRefreshToken(string refreshJwtToken)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(refreshJwtToken);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public bool VerfiyHashedJwtToken(string hashed, string refreshJwtToken)
        {
            var hashOfInput = HashRefreshToken(refreshJwtToken);
            return  hashed == hashOfInput;
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredJwtToken(string JwtToken)
        {
            var JwtSettings = _configuration.GetSection("Jwt");
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = JwtSettings["Issuer"],
                ValidAudience = JwtSettings["Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(_key),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(JwtToken, tokenValidationParameters, out var securityToken);
                if (securityToken is not JwtSecurityToken jwt || 
                    !jwt.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                        return null;
                return principal;
            }
            catch
            {
                return null;
            }
        }





    }
}
