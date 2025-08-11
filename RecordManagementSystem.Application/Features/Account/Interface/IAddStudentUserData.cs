using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecordManagementSystem.Application.Features.Account.DTO;
using RecordManagementSystem.Domain.Entities.Account;


namespace RecordManagementSystem.Application.Features.Account.Interface
{
    public interface IAddStudentUserData
    {
        Task<AddStudentAccountDTO> AddStudentAccount(AddStudentAccountDTO add);
        Task<AddStudentAccountDTO> GetStudentUserId(int Id);
        Task<IEnumerable<GetStudentAccountDTO>> GetAllStudentAccount();
        Task<bool> RegisterStudentAccount(RegisterStudentAccountDTO registerDTO);
        Task<bool> Login(LoginDTO loginDTO);
        Task Logout();
    }
}
