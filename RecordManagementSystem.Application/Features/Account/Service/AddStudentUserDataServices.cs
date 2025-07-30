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

        public async Task<Result<StudentUserData>> GetIdUsers(int id)
        {
            var User = await _addStudentUserData.GetUserId(id);
            if(User != null)
            {
                return Result<StudentUserData>.Ok(User);
            }
            else
            {
                return Result<StudentUserData>.Fail("Not found!");
            }
        }

        public async Task<AddStudentUserDataDTO> AddStudentData(AddStudentUserDataDTO add)
        {
            return await _addStudentUserData.AddStudent(add);
        }

    }
}
