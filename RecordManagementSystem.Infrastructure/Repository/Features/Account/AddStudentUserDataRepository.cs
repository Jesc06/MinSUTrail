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

        public async Task<StudentUserData> GetUserId(int id)
        {
            var UserId = _context.studentUserData.FirstOrDefault(Users => Users.Id == id);
            return UserId;
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
