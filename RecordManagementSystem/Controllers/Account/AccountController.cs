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
        private readonly AddStudentUserDataServices _services;
        private readonly IMapper _mapper;
        public AccountController(AddStudentUserDataServices services, IMapper mapper)
        {
            _services = services;
            _mapper = mapper;
        }


        [HttpGet("{id}")]
        public ActionResult<AddStudentUserDataDTO> GetId(int id)
        {
            var user = _services.GetIdUsers(id);
            return Ok(user);
        }


        [HttpPost("AddStudentUserData")]
        public async Task<ActionResult> AddStudentUsersData([FromBody]AccountDTO userData) 
        {
            if (ModelState.IsValid)
            {
                var addStudent = _mapper.Map<AddStudentUserDataDTO>(userData);
                var UserId = await _services.AddStudentData(addStudent);

                return CreatedAtAction(nameof(GetId), new { id = UserId.Id }, UserId);
            }
            return BadRequest();
        }


    }
}
