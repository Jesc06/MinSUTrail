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
        private static AddStudentAccountDTO add;

        private static Random generateOTP = new Random();
        private static int generated;

        public LoginRegisterController(IEmailService emailService, AddStudentUserAccountServices AddStudentAccountservices, AuthServices authServices, RefreshTokenServiceApp refreshTokenServiceApp, GenerateToken generateToken)
        {
            _AddStudentAccountservices = AddStudentAccountservices;
            _authServices = authServices;
            _refreshTokenServiceApp = refreshTokenServiceApp;
            _generateToken = generateToken;
            _emailService = emailService;
        }


        [HttpPost("AddStudentAccount")]
        public async Task<ActionResult> AddStudentAccount([FromBody] AddAccountDTO addAccountDTO)
        {

            if (ModelState.IsValid)
            {
                AddStudentAccountDTO addAccount = new AddStudentAccountDTO
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
                add = addAccount;
            }
            generated = generateOTP.Next();
            await _emailService.SendEmailAsync(addAccountDTO.Email, "Minsu", generated);
            return Ok();

        }



        [HttpPost("OTP")]
        public async Task<ActionResult> OTPRequest(EmailRequestDTO req)
        {
            if(generated == req.OTP)
            {
                var studentAccount = await _AddStudentAccountservices.AddStudentAccount(add);
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