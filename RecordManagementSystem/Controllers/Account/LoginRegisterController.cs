using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecordManagementSystem.Application.Features.Account.Service;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using RecordManagementSystem.Application.Features.Account.DTO;
using RecordManagementSystem.DTOs.Account;
using Azure.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using RecordManagementSystem.Infrastructure.Services;
using RecordManagementSystem.Application.Features.OTP.Services;
using RecordManagementSystem.Application.Features.OTP.Interfaces;
using System.Runtime.CompilerServices;
using RecordManagementSystem.Application.Features.OTP.DTO;
using RecordManagementSystem.DTOs.OTP;


namespace RecordManagementSystem.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginRegisterController : ControllerBase
    {
        private readonly AddStudentUserAccountServices _AddStudentAccountservices;
        private readonly RefreshTokenServiceApp _refreshTokenServiceApp;
        private readonly GenerateToken _generateToken;
        private readonly AuthServices _authServices;
        private readonly IEmailService _emailService;

        
        public LoginRegisterController(IEmailService emailService, AddStudentUserAccountServices AddStudentAccountservices, AuthServices authServices, RefreshTokenServiceApp refreshTokenServiceApp, GenerateToken generateToken)
        {
            _AddStudentAccountservices = AddStudentAccountservices;
            _authServices = authServices;
            _refreshTokenServiceApp = refreshTokenServiceApp;
            _generateToken = generateToken;
            _emailService = emailService;
        }


        [HttpPost("AddStudentAccount")]
        public async Task<ActionResult> AddStudentAccount([FromBody] OTPRequestApiDTOs  addAccountDTO)
        {

            if (!ModelState.IsValid)
                return BadRequest();

            OTPRequestDTO account = new OTPRequestDTO
            {
                FirstName = addAccountDTO.FirstName,
                Middlename = addAccountDTO.Middlename,
                LastName = addAccountDTO.LastName,
                Gender = addAccountDTO.Gender,
                YearOfBirth = addAccountDTO.YearOfBirth,
                MonthOfBirth = addAccountDTO.MonthOfBirth,
                DateOfBirth = addAccountDTO.DateOfBirth,
                HomeAddress = addAccountDTO.HomeAddress,
                MobileNumber = addAccountDTO.MobileNumber,
                Email = addAccountDTO.Email,
                Program = addAccountDTO.Program,
                YearLevel = addAccountDTO.YearLevel,
                StudentID = addAccountDTO.StudentID,
                Password = addAccountDTO.Password,
            };

            var ot = await _AddStudentAccountservices.AddStudentAccount(account);
            return Ok(new { 
                SessionId = ot.SessionID,
                ExpiryTime = ot.ExpiryTime
            });

        }



        [HttpPost("VerifyOTP")]
        public async Task<ActionResult> VerifyOTP(OTPVerification verifyOTP)
        {
            VerifyOTPDTO verify = new VerifyOTPDTO
            {
                sessionID = verifyOTP.sessionId,
                OTP = verifyOTP.OTP
            };
            var OTP = await _AddStudentAccountservices.VerifyOTP(verify);
            if (OTP)
            {
                return Created();
            }
            return BadRequest();
        }


        [HttpPost("RegisterStudentAccount")]
        public async Task<ActionResult> RegisterStudentAccount(RegisterDTO registerDto)
        {
            if (ModelState.IsValid)
            {
                RegisterStudentAccountDTO register = new RegisterStudentAccountDTO
                {
                    Id = registerDto.Id,
                    FirstName = registerDto.FirstName,
                    MiddleName = registerDto.Middlename,
                    LastName = registerDto.LastName,
                    Email = registerDto.Email,
                    Password = registerDto.Password,    
                };
                var IsRegister = await _authServices.RegisterStudentAccount(register);
                if (IsRegister)
                {
                    return Created();
                }
                return BadRequest();
            }
            return BadRequest(ModelState.ValidationState);    
        }


        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginApiDTO loginDTO)
        {
            if (ModelState.IsValid)
            {
                LoginDTO login = new LoginDTO
                {
                    Email = loginDTO.Email,
                    Password = loginDTO.Password
                };
                var IsLogin = await _authServices.Login(login);

                return Ok(IsLogin);
            }
            return BadRequest(ModelState.ValidationState);
        }


        [HttpPost("Refresh Token")]
        public async Task<ActionResult> RefreshToken([FromBody] JwtRefreshTokenDTO request)
        {
            var savedToken = await _refreshTokenServiceApp.GetByTokenAsync(request.RefreshToken);
            if(savedToken == null || savedToken.ExpiryDate < DateTime.UtcNow || savedToken.IsRevoked)
            {
                return Unauthorized();
            }

            var newTokens = _generateToken.Token(savedToken.Username, "Admin");
            savedToken.Token = newTokens.RefreshToken;
            savedToken.ExpiryDate = newTokens.RefreshTokenExpiry;
            await _refreshTokenServiceApp.UpdateAsync(savedToken);

            return Ok(newTokens);

        }
        

        [HttpPost("Logout")]
        public async Task<ActionResult> Logout()
        {
            await _authServices.Logout();
            return Ok(new { message= "Logged out!" });
        }



    }
}