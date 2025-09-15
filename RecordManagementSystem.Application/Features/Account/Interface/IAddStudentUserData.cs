using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecordManagementSystem.Application.Features.Account.DTO;
using RecordManagementSystem.Application.Features.OTP.DTO;
using RecordManagementSystem.Domain.Entities.Account;
using RecordManagementSystem.Domain.Entities.OTP;


namespace RecordManagementSystem.Application.Features.Account.Interface
{
    public interface IAddStudentUserData
    {
        Task<OTPRequest> AddStudentAccount(OTPRequestDTO addAccountDTO);
        Task<IEnumerable<GetStudentAccountDTO>> GetAllStudentAccount();
        Task<bool> VerifyOTP(VerifyOTPDTO verifyOTP);
    }
}
