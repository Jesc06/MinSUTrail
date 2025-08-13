using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecordManagementSystem.DTO.Account;
using RecordManagementSystem.Application.Features.Account.Service;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using RecordManagementSystem.Application.Features.Account.DTO;
using Azure.Identity;

namespace RecordManagementSystem.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AddStudentUserAccountServices _services;
        private readonly AuthServices _authServices;
        public AccountController(AddStudentUserAccountServices services, AuthServices authServices)
        {
            _services = services;
            _authServices = authServices;
        }


        [HttpGet("{id}")]
        public ActionResult GetUserId(int id)
        {
            var user = _services.GetStudentUserId(id);
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
                var UserId = await _services.AddStudentAccount(addAccount);
  
                return CreatedAtAction(nameof(GetUserId), new { id = UserId.Id }, UserId);
            }
            return BadRequest();
        }


        [HttpGet("GetAllStudentAccount")]
        public async Task<ActionResult> GetAllStudentAccount()
        {
            var GetAllAccounts = await _services.GetAllStudentAccounts();
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
            return BadRequest();    
        }


        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginApiDTO loginDTO)
        {
            LoginDTO login = new LoginDTO
            {
                Email = loginDTO.Email,
                Password = loginDTO.Password
            };
            var IsLogin = await _authServices.Login(login);
            return Ok(IsLogin);
        }


        [HttpPost("Logout")]
        public async Task<ActionResult> Logout()
        {
            await _authServices.Logout();
            return Ok();
        }



    }
}
