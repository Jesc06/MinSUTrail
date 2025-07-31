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
    public class AddStudentUserDataServices
    {
        private readonly IAddStudentUserData _addStudentUserData;
        public AddStudentUserDataServices(IAddStudentUserData addStudentUserData)
        {
            _addStudentUserData = addStudentUserData;
        }

        public async Task<Result<AddStudentAccountDTO>> GetUsersId(int id)
        {
            var UserId = await _addStudentUserData.GetUserId(id);
            if(UserId != null)
            {
                return Result<AddStudentAccountDTO>.Ok(UserId);
            }
            return Result<AddStudentAccountDTO>.Fail("Not found!");
        }


        public async Task<AddStudentAccountDTO> AddStudentAccount(AddStudentAccountDTO add)
        {
            return await _addStudentUserData.AddStudentAccount(add);
        }

    }
}
