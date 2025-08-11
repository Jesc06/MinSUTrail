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
        private readonly IGenerateTokenService _generateTokenService;
        public AddStudentUserAccountServices(IGenerateTokenService generateTokenService,IAddStudentUserData servicesData)
        {
            _servicesData = servicesData;
            _generateTokenService = generateTokenService;
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

        public async Task<bool> RegisterStudentAccount(RegisterStudentAccountDTO registerAccount)
        {
            var IsRegister = await _servicesData.RegisterStudentAccount(registerAccount);
            if (IsRegister)
            {
                return true;
            }
            return false;
        }


        public async Task<string> Login(LoginDTO loginDTO)
        {
            var isLogin = await _servicesData.Login(loginDTO);
            if (isLogin)
            {
                return _generateTokenService.GenerateToken(loginDTO.Email, "Student");
            }
            throw new UnauthorizedAccessException("Invalid credentials!");
        }


        public async Task Logout()
        {
            await _servicesData.Logout();
        }



    }
}
