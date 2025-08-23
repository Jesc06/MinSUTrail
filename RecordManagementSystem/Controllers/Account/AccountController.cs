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


namespace RecordManagementSystem.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AddStudentUserAccountServices _AddStudentAccountservices;
        private readonly AuthServices _authServices;
        public AccountController(AddStudentUserAccountServices AddStudentAccountservices, AuthServices authServices)
        {
            _AddStudentAccountservices = AddStudentAccountservices;
            _authServices = authServices;
        }


        [HttpGet("{id}")]
        public ActionResult GetUserId(int id)
        {
            var user = _AddStudentAccountservices.GetStudentUserId(id);
            return Ok(user);
        }


        [HttpPost("AddStudentAccount")]
        public async Task<ActionResult> AddStudentAccount([FromBody]AddAccountDTO addAccountDTO) 
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
                var studentAccount = await _AddStudentAccountservices.AddStudentAccount(addAccount);
  
                return CreatedAtAction(nameof(GetUserId), new { id = studentAccount.Id }, studentAccount);
            }
            return BadRequest(ModelState.ValidationState);
        }


        [HttpGet("GetAllStudentAccount")]
        public async Task<ActionResult> GetAllStudentAccount()
        {
            var GetAllAccounts = await _AddStudentAccountservices.GetAllStudentAccounts();
            return Ok(GetAllAccounts);
        }


        [HttpPost("RegisterStudentAccount")]
        public async Task<ActionResult> RegisterStudentAccount(RegisterDTO registerDto)
        {
            if (ModelState.IsValid)
            {
                RegisterStudentAccountDTO register = new RegisterStudentAccountDTO
                {
                    Id = registerDto.Id,
                    Firtsname = registerDto.FirstName,
                    MiddleName = registerDto.MiddleName,
                    LastName = registerDto.LastName,
                    Email = registerDto.Email,
                    Password = registerDto.Password
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


        [HttpPost("Logout")]
        public async Task<ActionResult> Logout()
        {
            Response.Cookies.Delete("Jwt");
            await _authServices.Logout();
            return Ok(new { message= "Logged out!" });
        }



    }
}