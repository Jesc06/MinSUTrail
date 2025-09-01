using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RecordManagementSystem.Application.Features.Account.Interface;
using RecordManagementSystem.Application.Features.Account.DTO;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;


namespace RecordManagementSystem.Infrastructure.Services
{
    public class GenerateTokenService : IGenerateTokenService
    {
        private readonly IConfiguration _config;
        public GenerateTokenService(IConfiguration config)
        {
            _config = config;
        }

        public TokenResponseDTO GenerateToken(string username, string role)
        {
            var Claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(ClaimTypes.Role, role), 
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:key"]));
            var Expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:ExpireMinutes"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: Claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:ExpireMinutes"])),
                signingCredentials: creds
            );
            var Token = new JwtSecurityTokenHandler().WriteToken(token);

            //Refresh token method
            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            return new TokenResponseDTO
            {
                Token = Token,  
                ExpiresIn = (int) (Expiration - DateTime.UtcNow).TotalSeconds,
                Role = role,

                RefreshToken = refreshToken,
                RefreshTokenExpiry = refreshTokenExpiry
            };

        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using (var range = RandomNumberGenerator.Create())
            {
                range.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }



    }
}