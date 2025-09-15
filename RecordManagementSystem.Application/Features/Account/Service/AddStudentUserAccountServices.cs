using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecordManagementSystem.Application.Features.Account.DTO;
using RecordManagementSystem.Application.Features.Account.Interface;
using RecordManagementSystem.Domain.Entities.Account;
using RecordManagementSystem.Application.Common.Models;
using RecordManagementSystem.Domain.Entities.OTP;
using RecordManagementSystem.Application.Features.OTP.DTO;

namespace RecordManagementSystem.Application.Features.Account.Service
{
    public class AddStudentUserAccountServices
    {
        private readonly IAddStudentUserData _servicesData;
        public AddStudentUserAccountServices(IAddStudentUserData servicesData)
        {
            _servicesData = servicesData;
        }

        public async Task<OTPRequest> AddStudentAccount(OTPRequestDTO addAccountDTO)
        {
            return await _servicesData.AddStudentAccount(addAccountDTO);
        }


        public async Task<bool> VerifyOTP(VerifyOTPDTO verify)
        {
            return await _servicesData.VerifyOTP(verify);
        }

        public async Task<IEnumerable<GetStudentAccountDTO>> GetAllStudentAccounts()
        {
            return await _servicesData.GetAllStudentAccount();
        }


    }
}
