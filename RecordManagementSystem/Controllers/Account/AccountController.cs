using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecordManagementSystem.DTO.Account;
using RecordManagementSystem.Application.Features.Account.Service;
using RecordManagementSystem.Application.Features.Account.DTO;
using AutoMapper;
using System.Threading.Tasks;

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

    

        [HttpPost("AddStudentUserData")]
        public async Task<IActionResult> AddStudentUsersData([FromBody]AccountDTO userData) 
        {
            if (ModelState.IsValid)
            {
                var addStudent = _mapper.Map<AddStudentUserDataDTO>(userData);
                await _services.AddStudentData(addStudent);
                return Ok(addStudent);  
            }
            return BadRequest();
        }
    }
}
