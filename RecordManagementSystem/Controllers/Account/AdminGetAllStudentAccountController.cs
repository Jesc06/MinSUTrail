using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecordManagementSystem.Application.Features.Account.Service;

namespace RecordManagementSystem.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminGetAllStudentAccountController : ControllerBase
    {
        private readonly AddStudentUserAccountServices _AddStudentAccountservices;
        public AdminGetAllStudentAccountController(AddStudentUserAccountServices addStudentUserAccountServices)
        {
            _AddStudentAccountservices = addStudentUserAccountServices;
        }

        [HttpGet("GetAllStudentAccount")]
        public async Task<ActionResult> GetAllStudentAccount()
        {
            var GetAllAccounts = await _AddStudentAccountservices.GetAllStudentAccounts();
            return Ok(GetAllAccounts);
        }

    }
}
