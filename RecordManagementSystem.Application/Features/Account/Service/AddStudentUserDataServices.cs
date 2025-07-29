using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecordManagementSystem.Application.Features.Account.DTO;
using RecordManagementSystem.Application.Features.Account.Interface;
using RecordManagementSystem.Domain.Entities.Account;

namespace RecordManagementSystem.Application.Features.Account.Service
{
    public class AddStudentUserDataServices
    {
        private readonly IAddStudentUserData _addStudentUserData;
        public AddStudentUserDataServices(IAddStudentUserData addStudentUserData)
        {
            _addStudentUserData = addStudentUserData;
        }

        public async Task<StudentUserData> GetIdUsers(int id)
        {
            return await _addStudentUserData.GetUserId(id);
        }
        public async Task AddStudentData(AddStudentUserDataDTO add)
        {
            await _addStudentUserData.AddStudent(add);
        }

    }
}
