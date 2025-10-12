using Microsoft.AspNetCore.Http;
using RecordManagementSystem.Application.Features.Account.DTO;
using RecordManagementSystem.Application.Features.Account.Interface;
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
        public AuthServices(IAuthService authService)
        {
            _authService = authService;
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

        public async Task<JwtTokenResponse> Login(LoginDTO loginDTO)
        {
            return await _authService.Login(loginDTO);
        }

        public async Task<JwtRefreshTokenResponse> RefreshToken(JwtRefreshTokenResponse refreshToken)
        {
            return await _authService.JwtRefreshToken(refreshToken);
        }

        public async Task Logout()
        {
            await _authService.Logout();
        }


    }
}
