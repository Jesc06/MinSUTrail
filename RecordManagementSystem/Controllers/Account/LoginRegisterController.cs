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
using RecordManagementSystem.DTOs.OTP;
using RecordManagementSystem.Application.Features.OTP.Interfaces;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Caching.Memory;


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
        private readonly IMemoryCache _cache;
        private static Random generateOTP = new Random();
    

        public LoginRegisterController(IMemoryCache cache,IEmailService emailService, AddStudentUserAccountServices AddStudentAccountservices, AuthServices authServices, RefreshTokenServiceApp refreshTokenServiceApp, GenerateToken generateToken)
        {
            _AddStudentAccountservices = AddStudentAccountservices;
            _authServices = authServices;
            _refreshTokenServiceApp = refreshTokenServiceApp;
            _generateToken = generateToken;
            _emailService = emailService;
            _cache = cache;
        }


        [HttpPost("AddStudentAccount")]
        public async Task<ActionResult> AddStudentAccount([FromBody] AddAccountDTO addAccountDTO)
        {

            if (!ModelState.IsValid)
                return BadRequest();

            //Generate OTP
            var otp = generateOTP.Next();

            var pending = new PendingOTPRequest
            {
                account = new AddStudentAccountDTO
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
                },
                OTP = otp
            };

            //OTP expiration
            var sessionID = Guid.NewGuid().ToString();
            _cache.Set(sessionID, pending,
                 new MemoryCacheEntryOptions
                 {
                     AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60)
                 }    
            );

            Response.Cookies.Append("OTPniJosh", sessionID);

            await _emailService.SendEmailAsync(addAccountDTO.Email, "Minsu", otp);
            return Ok(new {message = $"OTP successfully sent in {addAccountDTO.Email}"});
        }



        [HttpPost("VerifyOTP")]
        public async Task<ActionResult> OTPRequest(EmailRequestDTO req)
        {
            
            var sessionID = Request.Cookies["OTPniJosh"];
            if (string.IsNullOrWhiteSpace(sessionID))
                return BadRequest("No session found!");

            if(_cache.TryGetValue<PendingOTPRequest>(sessionID, out var pending))
            {
                if (pending.OTP == req.OTP)
                {
                    await _AddStudentAccountservices.AddStudentAccount(pending.account);
                    _cache.Remove(sessionID);
                    return Created();
                }
                return BadRequest("Invalid OTP");
            }
            return BadRequest("OTP is expired");
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

            var newTokens = _generateToken.Token(savedToken.Username, "Student");
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