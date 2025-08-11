using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RecordManagementSystem.Application.Features.Account.Interface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;


namespace RecordManagementSystem.Infrastructure.Services
{
    public class GenerateTokenService : IGenerateTokenService
    {
        private readonly IConfiguration _config;
        public GenerateTokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(string username, string role)
        {

            var Claims = new[]
     {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim("role", role), 
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: Claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:ExpireMinutes"])),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }



    }
}