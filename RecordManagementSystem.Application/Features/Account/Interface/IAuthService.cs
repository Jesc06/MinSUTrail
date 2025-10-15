using RecordManagementSystem.Application.Common.Models;
using RecordManagementSystem.Application.Features.Account.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordManagementSystem.Application.Features.Account.Interface
{
    public interface IAuthService
    {
        Task<bool> RegisterStudentAccount(RegisterStudentAccountDTO registerDTO);
        Task<GenerateJwtTokenResponseDTO> Login(LoginDTO loginDTO);
        Task<Result<JwtRefreshTokenResponseDTO>> JwtRefreshToken(JwtRefreshTokenRequestDTO tokenResponse);
        Task Logout();
    }
}
