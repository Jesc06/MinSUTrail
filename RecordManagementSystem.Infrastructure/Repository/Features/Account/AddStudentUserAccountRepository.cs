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
        public AddStudentUserAccountRepository(ApplicationDbContext context)
        {
            _context = context;
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
