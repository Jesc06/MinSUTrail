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
            var UserIdExistence = _context.studentUserAccount.FirstOrDefault(Users => Users.Id == id);

            if (UserIdExistence is null) { return null; }

            var studentAccountDTO = new AddStudentAccountDTO
            {
                Id = UserIdExistence.Id,
                FirstName = UserIdExistence.FirstName,
                Middlename = UserIdExistence.Middlename,
                LastName = UserIdExistence.LastName,
                Gender = UserIdExistence.Gender,
                YearOfBirth = UserIdExistence.YearOfBirth,   
                MonthOfBirth = UserIdExistence.MonthOfBirth,
                DateOfBirth = UserIdExistence.DateOfBirth,
                HomeAddress = UserIdExistence.HomeAddress,
                MobileNumber = UserIdExistence.MobileNumber,
                Email = UserIdExistence.Email,
                Program = UserIdExistence.Program,
                YearLevel = UserIdExistence.YearLevel,   
                StudentID = UserIdExistence.StudentID,
                Password = UserIdExistence.Password,
            };

            return studentAccountDTO;
        }


        public async Task<IEnumerable<GetStudentAccountDTO>> GetAllStudentAccount()
        {
            return await _context.studentUserAccount.Select(studentUserAccount => new GetStudentAccountDTO
            {
                Id = studentUserAccount.Id,
                FirstName = studentUserAccount.FirstName,
                Middlename = studentUserAccount.Middlename,
                LastName = studentUserAccount.LastName,
                Gender = studentUserAccount.Gender,
                YearOfBirth = studentUserAccount.YearOfBirth,
                MonthOfBirth = studentUserAccount.MonthOfBirth,
                DateOfBirth = studentUserAccount.DateOfBirth,
                HomeAddress = studentUserAccount.HomeAddress,
                MobileNumber = studentUserAccount.MobileNumber,
                Email = studentUserAccount.Email,
                Program = studentUserAccount.Program,
                YearLevel = studentUserAccount.YearLevel,
                StudentID = studentUserAccount.StudentID,
                Password = studentUserAccount.Password
            }).ToListAsync();
        }



    }
}
