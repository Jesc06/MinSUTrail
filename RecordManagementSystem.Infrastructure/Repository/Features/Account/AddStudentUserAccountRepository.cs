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
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Azure;


namespace RecordManagementSystem.Infrastructure.Repository.Features.Account
{
    public class AddStudentUserAccountRepository : IAddStudentUserData 
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserIdentity> _userManager;
        private readonly IMapper _mapper;
        public AddStudentUserAccountRepository(IMapper mapper,ApplicationDbContext context, UserManager<UserIdentity> userManager)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

      
        public async Task<AddStudentAccountDTO> AddStudentAccount(AddStudentAccountDTO addStudentDTO)
        {
            var addStudentAccount = _mapper.Map<StudentUserAccount>(addStudentDTO);
            await _context.AddAsync(addStudentAccount);
            await _context.SaveChangesAsync();

            addStudentDTO.Id = addStudentAccount.Id;
            return addStudentDTO;
        }


        public async Task<AddStudentAccountDTO> GetUserId(int id)
        {
            var UserId = _context.studentUserAccount.FirstOrDefault(Users => Users.Id == id);

            if (UserId == null) { return null; }

            var UserData = _mapper.Map<AddStudentAccountDTO>(UserId);

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
                StudentID = Users.StudentID
            }).ToListAsync();

        }




    }
}
