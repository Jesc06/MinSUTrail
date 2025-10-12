using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RecordManagementSystem.Application.Common.Models;
using RecordManagementSystem.Application.Features.Account.DTO;
using RecordManagementSystem.Application.Features.Account.Interface;
using RecordManagementSystem.Infrastructure.Persistence.Data;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthService(SignInManager<UserIdentity> signInManager, 
                           UserManager<UserIdentity> userManager,
                           ApplicationDbContext context,
                           RoleManager<IdentityRole> roleManager,
                           IJwtToken jwtToken,
                           IConfiguration configuration,
                           IHttpContextAccessor httpContextAccessor)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
            _jwtToken = jwtToken;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
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

            if (findUser is null) return null;

            var isLogin = await _userManager.CheckPasswordAsync(findUser,loginDTO.Password);

            var getUserRoles = await _userManager.GetRolesAsync(findUser);

            JwtApplicationUser user = new JwtApplicationUser
            {
                id = findUser.Id,
                username = findUser.UserName,
                email = findUser.Email,
                Roles = getUserRoles
            };

            if (isLogin)
            {
                var accessToken = _jwtToken.GenerateAccessJwtToken(user);
                var refreshToken = _jwtToken.GenerateRefreshJwtToken();

                findUser.RefreshTokenHash = _jwtToken.HashRefreshToken(refreshToken);
                findUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:RefreshTokenDurationInDays"] ?? "2"));
                await _userManager.UpdateAsync(findUser);
                
                return new JwtTokenResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                };
            }
            return null;
        }


        public async Task<Result<JwtRefreshTokenResponse>> JwtRefreshToken(JwtRefreshTokenRequest tokenRequest)
        {
            // DEBUG: log all claims in the token
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenRequest.newAccessToken);

            foreach (var c in token.Claims)
            {
                Console.WriteLine($"{c.Type} = {c.Value}");
            }

            var principal = _jwtToken.GetPrincipalFromExpiredJwtToken(tokenRequest.newAccessToken);
            if (principal is null)
                return Result<JwtRefreshTokenResponse>.Fail("Principal is null");

            var username = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                ?? principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? principal.FindFirst("name")?.Value;

            if (username is null)
                return Result<JwtRefreshTokenResponse>.Fail("Principal username cannot find");


            var user = await _userManager.FindByNameAsync(username);
            if (user is null)
                return Result<JwtRefreshTokenResponse>.Fail("username cannot exist");

            // Verify refresh token validity
            var isValidRefreshToken = _jwtToken.VerfiyHashedJwtToken(user.RefreshTokenHash, tokenRequest.newRefreshToken);
            if (!isValidRefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return Result<JwtRefreshTokenResponse>.Fail("refresh token not valid");

            // Generate new tokens
            var newAccessToken = _jwtToken.GenerateAccessJwtToken(new JwtApplicationUser
            {
                id = user.Id,
                username = user.UserName,
                email = user.Email,
                Roles = await _userManager.GetRolesAsync(user)
            });

            var newRefreshToken = _jwtToken.GenerateRefreshJwtToken();

            // Update stored refresh token hash and expiry
            user.RefreshTokenHash = _jwtToken.HashRefreshToken(newRefreshToken);
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:RefreshTokenDurationInDays"] ?? "2"));
            await _userManager.UpdateAsync(user);

            var response = new JwtRefreshTokenResponse
            {
                newAccessToken = newAccessToken,
                newRefreshToken = newRefreshToken
            };

            return Result<JwtRefreshTokenResponse>.Ok(response);

        }

        public async Task Logout()
        {
            var user = await _userManager.FindByEmailAsync("escarezjohnjoshuamanalo@gmail.com");
            if (user is  not null)
            {
                user.RefreshTokenHash = null;
                user.RefreshTokenExpiryTime = null;
                await _userManager.UpdateAsync(user);

                await _signInManager.SignOutAsync();
            }
        }


    }
}
