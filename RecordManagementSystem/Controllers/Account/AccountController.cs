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
        public AccountController(AddStudentUserAccountServices services)
        {
            _services = services;
        }


        [HttpGet("{id}")]
        public ActionResult<AddAccountDTO> GetUserId(int id)
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
                    Firtsname = registerDto.FirstName,
                    MiddleName = registerDto.MiddleName,
                    LastName = registerDto.LastName,
                    Email = registerDto.Email,
                    Password = registerDto.Password
                };
                await _services.RegisterStudentAccount(register);
                return Created();
            }
            return BadRequest();    
        }



    }
}
