using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RecordManagementSystem.Application.Features.Account.DTO;
using RecordManagementSystem.Application.Features.Account.Interface;
using RecordManagementSystem.Infrastructure.Persistence.Data;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordManagementSystem.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<UserIdentity> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserIdentity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtToken _jwtToken;
        private readonly IConfiguration _configuration;
        public AuthService(SignInManager<UserIdentity> signInManager, 
                           UserManager<UserIdentity> userManager,
                           ApplicationDbContext context,
                           RoleManager<IdentityRole> roleManager,
                           IJwtToken jwtToken,
                           IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
            _jwtToken = jwtToken;
            _configuration = configuration;
        }

        public async Task<bool> RegisterStudentAccount(RegisterStudentAccountDTO registerAccount)
        {
            UserIdentity userDataIdentity = new UserIdentity
            {
                FirstName = registerAccount.FirstName,
                MiddleName = registerAccount.MiddleName,
                LastName = registerAccount.LastName,
                Email = registerAccount.Email,
                UserName = registerAccount.Email
            };
            var isRegister = await _userManager.CreateAsync(userDataIdentity, registerAccount.Password);
            if (isRegister.Succeeded)
            {
                await _userManager.AddToRoleAsync(userDataIdentity, "Student");
                var find = _context.studentUserAccount.Find(registerAccount.Id);
                if (find is not null)
                {
                    _context.studentUserAccount.Remove(find);
                    _context.SaveChanges();
                }
                return true;
            }
            if (!isRegister.Succeeded)
            {
                foreach (var error in isRegister.Errors)
                {
                    Console.WriteLine(error.Description); // o i-log sa ILogger
                }
                return false;
            }
            return false;
        }

        public async Task<JwtTokenResponse> Login(LoginDTO loginDTO)
        {
            var findUser = await _userManager.FindByEmailAsync(loginDTO.Email);
            var isLogin = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, true, true);

            var getUserRoles = await _userManager.GetRolesAsync(findUser);

            JwtApplicationUser user = new JwtApplicationUser
            {
                id = findUser.Id,
                username = findUser.UserName,
                email = findUser.Email,
                Roles = getUserRoles
            };

            if (isLogin.Succeeded)
            {
                var accessToken = _jwtToken.GenerateAccessJwtToken(user);
                var refreshToken = _jwtToken.GenerateRefreshJwtToken();

                findUser.RefreshTokenHash = _jwtToken.HashRefreshToken(refreshToken);
                findUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(int.Parse(_configuration["Jwt:RefreshTokenDurationInDays"] ?? "14"));
                await _userManager.UpdateAsync(findUser);

                return new JwtTokenResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                };
            }
            return null;
        }


        public async Task<JwtRefreshTokenResponse> JwtRefreshToken(JwtRefreshTokenResponse tokenResponse)
        {
            if(tokenResponse is null) return null;

            var principal = _jwtToken.GetPrincipalFromExpiredJwtToken(tokenResponse.newAccessToken);
            if(principal is null) return null;

            var username = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (username is null) return null;

            var user = await _userManager.FindByNameAsync(username);
            if (user is null) return null;

            if (user.RefreshTokenHash is null || user.RefreshTokenExpiryTime is null) return null;
            if(user.RefreshTokenExpiryTime <= DateTime.UtcNow) return null; 

            var isValidRefreshToken = _jwtToken.VerfiyHashedJwtToken(user.RefreshTokenHash,tokenResponse.newRefreshToken);
            if(!isValidRefreshToken)return null;

            var roles = await _userManager.GetRolesAsync(user);
            var JwtApplicationUsers = new JwtApplicationUser
            {
                id = user.Id,
                username = user.UserName,
                email = user.Email,
                Roles = roles
            };

            var newAccessToken = _jwtToken.GenerateAccessJwtToken(JwtApplicationUsers);
            var newRefreshToken = _jwtToken.GenerateRefreshJwtToken();

            var newRefreshTokenHash = _jwtToken.HashRefreshToken(newRefreshToken);
            var newRefreshTokenExpiryTime = DateTime.UtcNow.AddDays(int.Parse(_configuration["Jwt:RefreshTokenDurationInDays"] ?? "14"));
            await _userManager.UpdateAsync(user);

            return new JwtRefreshTokenResponse
            {
                newAccessToken = newAccessToken,
                newRefreshToken = newRefreshToken
            };

        }


        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

    }
}
