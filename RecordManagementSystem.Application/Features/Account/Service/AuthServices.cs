using Microsoft.AspNetCore.Http;
using RecordManagementSystem.Application.Features.Account.DTO;
using RecordManagementSystem.Application.Features.Account.Interface;
using RecordManagementSystem.Domain.Entities.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordManagementSystem.Application.Features.Account.Service
{
    public class AuthServices
    {
        private readonly IAuthService _authService;
        private readonly IGenerateTokenService _generateTokenService;
        private readonly IRefreshToken _refreshToken;
        public AuthServices(IAuthService authService, IGenerateTokenService generateTokenService, IRefreshToken refreshToken)
        {
            _authService = authService;
            _generateTokenService = generateTokenService;
            _refreshToken = refreshToken;
        }

        public async Task<bool> RegisterStudentAccount(RegisterStudentAccountDTO registerAccount)
        {
            var isRegister = await _authService.RegisterStudentAccount(registerAccount);
            if (isRegister)
            {
                return true;
            }
            return false;
        }

        public async Task<TokenResponseDTO> Login(LoginDTO loginDTO)
        {
            var isLogin = await _authService.Login(loginDTO);
            if (isLogin)
            {
                var token = _generateTokenService.GenerateToken(loginDTO.Email, "Student");

                var refreshToken = new RefreshToken
                {
                    Username = loginDTO.Email,
                    Token = token.RefreshToken,
                    ExpiryDate = token.RefreshTokenExpiry,
                    IsRevoked = false
                };

                await _refreshToken.AddAsync(refreshToken); 
                return token;   
            }
            throw new UnauthorizedAccessException("Invalid credentials!");
        }


        public async Task<TokenResponseDTO> RefreshToken(RefreshTokenDTO refreshTokenDTO)
        {
            var savedToken = await _refreshToken.GetByTokenAsync(refreshTokenDTO.RefreshToken);
            if(savedToken is null || savedToken.ExpiryDate < DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Invalid or expired refresh token");
            }

            var newTokens = _generateTokenService.GenerateToken(savedToken.Username, "Student");

            savedToken.Token = newTokens.RefreshToken;
            savedToken.ExpiryDate = newTokens.RefreshTokenExpiry;
            await _refreshToken.UpdateAsync(savedToken);

            return newTokens;

        }

        public async Task Logout()
        {
            await _authService.Logout();
        }


    }
}
