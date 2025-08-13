using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecordManagementSystem.Application.Features.Account.DTO;
using RecordManagementSystem.Application.Features.Account.Interface;
using RecordManagementSystem.Domain.Entities.Account;
using RecordManagementSystem.Application.Common.Models;

namespace RecordManagementSystem.Application.Features.Account.Service
{
    public class AddStudentUserAccountServices
    {
        private readonly IAddStudentUserData _servicesData;
        public AddStudentUserAccountServices(IAddStudentUserData servicesData)
        {
            _servicesData = servicesData;
        }

        public async Task<AddStudentAccountDTO> AddStudentAccount(AddStudentAccountDTO add)
        {
            return await _servicesData.AddStudentAccount(add);
        }

        public async Task<Result<AddStudentAccountDTO>> GetStudentUserId(int id)
        {
            var UserId = await _servicesData.GetStudentUserId(id);
            if (UserId is not null)
            {
                return Result<AddStudentAccountDTO>.Ok(UserId);
            }
            return Result<AddStudentAccountDTO>.Fail("Not found!");
        }

        public async Task<IEnumerable<GetStudentAccountDTO>> GetAllStudentAccounts()
        {
            return await _servicesData.GetAllStudentAccount();
        }



    }
}
