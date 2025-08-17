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
        private readonly IGenerateTokenService _generateTokenService;
        public AuthServices(IAuthService authService, IGenerateTokenService generateTokenService)
        {
            _authService = authService;
            _generateTokenService = generateTokenService;
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
                return _generateTokenService.GenerateToken(loginDTO.Email, "Student");
            }
            throw new UnauthorizedAccessException("Invalid credentials!");
        }

        public async Task Logout()
        {
            await _authService.Logout();
        }

    }
}
