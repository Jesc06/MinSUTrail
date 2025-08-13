using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.Internal;
using RecordManagementSystem.Application.Features.Account.Interface;
using RecordManagementSystem.Infrastructure.Persistence.Data;
using RecordManagementSystem.Application.Features.Account.DTO;
using RecordManagementSystem.Domain.Entities.Account;
using RecordManagementSystem.Application.Common.Models;
using RecordManagementSystem.Infrastructure.Persistence.Seeder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Azure;

namespace RecordManagementSystem.Infrastructure.Repository.Features.Account
{
    public class AddStudentUserAccountRepository : IAddStudentUserData 
    {

        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AddStudentUserAccountRepository(RoleManager<IdentityRole> roleManager,ApplicationDbContext context)
        {
            _context = context;
            _roleManager = roleManager;
        }

        public async Task<AddStudentAccountDTO> AddStudentAccount(AddStudentAccountDTO addStudentDTO)
        {
            var addStudentAccount = new StudentUserAccount
            {
                FirstName = addStudentDTO.FirstName,
                Middlename = addStudentDTO.Middlename,
                LastName = addStudentDTO.LastName,
                Gender = addStudentDTO.Gender,
                YearOfBirth = addStudentDTO.YearOfBirth,
                MonthOfBirth = addStudentDTO.MonthOfBirth,
                DateOfBirth = addStudentDTO.DateOfBirth,
                HomeAddress = addStudentDTO.HomeAddress,
                MobileNumber = addStudentDTO.MobileNumber,
                Email = addStudentDTO.Email,
                Program = addStudentDTO.Program,
                YearLevel = addStudentDTO.YearLevel,
                StudentID = addStudentDTO.StudentID,
                Password = addStudentDTO.Password
            };
            await _context.studentUserAccount.AddAsync(addStudentAccount);
            await _context.SaveChangesAsync();

            addStudentDTO.Id = addStudentAccount.Id;
            return addStudentDTO;
        }


        public async Task<AddStudentAccountDTO> GetStudentUserId(int id)
        {
            var UserId = _context.studentUserAccount.FirstOrDefault(Users => Users.Id == id);

            if (UserId == null) { return null; }

            var UserData = new AddStudentAccountDTO
            {
                Id = UserId.Id,
                FirstName = UserId.FirstName,
                Middlename = UserId.Middlename,
                LastName = UserId.LastName,
                Gender = UserId.Gender,
                YearOfBirth = UserId.YearOfBirth,   
                MonthOfBirth = UserId.MonthOfBirth,
                DateOfBirth = UserId.DateOfBirth,
                HomeAddress = UserId.HomeAddress,
                MobileNumber = UserId.MobileNumber,
                Email = UserId.Email,
                Program = UserId.Program,
                YearLevel = UserId.YearLevel,   
                StudentID = UserId.StudentID,
                Password = UserId.Password,
            };

            return UserData;
        }


        public async Task<IEnumerable<GetStudentAccountDTO>> GetAllStudentAccount()
        {
            return await _context.studentUserAccount.Select(Users => new GetStudentAccountDTO
            {
                Id = Users.Id,
                FirstName = Users.FirstName,
                Middlename = Users.Middlename,
                LastName = Users.LastName,
                Gender = Users.Gender,
                YearOfBirth = Users.YearOfBirth,
                MonthOfBirth = Users.MonthOfBirth,
                DateOfBirth = Users.DateOfBirth,
                HomeAddress = Users.HomeAddress,
                MobileNumber = Users.MobileNumber,
                Email = Users.Email,
                Program = Users.Program,
                YearLevel = Users.YearLevel,
                StudentID = Users.StudentID,
                Password = Users.Password
            }).ToListAsync();
        }



    }
}
