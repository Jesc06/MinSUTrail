using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecordManagementSystem.DTO.Account;
using RecordManagementSystem.Application.Features.Account.Service;
using RecordManagementSystem.Application.Features.Account.DTO;
using AutoMapper;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Storage.Internal;

namespace RecordManagementSystem.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AddStudentUserAccountServices _services;
        private readonly IMapper _mapper;
        public AccountController(AddStudentUserAccountServices services, IMapper mapper)
        {
            _services = services;
            _mapper = mapper;
        }


        [HttpGet("{id}")]
        public ActionResult<AddStudentAccountDTO> GetUserId(int id)
        {
            var user = _services.GetUsersId(id);
            return Ok(user);
        }


        [HttpPost("AddStudentAccount")]
        public async Task<ActionResult> AddStudentAccount([FromBody]AccountDTO userData) 
        {
            if (ModelState.IsValid)
            {
                var addStudent = _mapper.Map<AddStudentAccountDTO>(userData);
                var UserId = await _services.AddStudentAccount(addStudent);

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


    }
}
