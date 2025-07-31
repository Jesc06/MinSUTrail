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
    public class AddStudentUserDataRepository : IAddStudentUserData 
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserIdentity> _userManager;
        private readonly IMapper _mapper;
        public AddStudentUserDataRepository(IMapper mapper,ApplicationDbContext context, UserManager<UserIdentity> userManager)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<AddStudentUserDataDTO> GetUserId(int id)
        {
            var UserId = _context.studentUserData.FirstOrDefault(Users => Users.Id == id);

            if(UserId == null) { return null; }

            var dto = new AddStudentUserDataDTO
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
        
            return dto;
        }


        public async Task<AddStudentUserDataDTO> AddStudent(AddStudentUserDataDTO addStudentDTO)
        {
       
            var addStudent = _mapper.Map<StudentUserData>(addStudentDTO);
            await _context.AddAsync(addStudent);
            await _context.SaveChangesAsync();

            addStudentDTO.Id = addStudent.Id;
            return addStudentDTO;
        }


    }
}
