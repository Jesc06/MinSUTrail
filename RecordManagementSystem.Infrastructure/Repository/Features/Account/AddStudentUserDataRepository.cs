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
using AutoMapper;
using Microsoft.AspNetCore.Identity;


namespace RecordManagementSystem.Infrastructure.Repository.Features.Account
{
    public class AddStudentUserDataRepository : IAddStudentUserData
    {
        private readonly ApplicationDbContext _context;
  
        private readonly IMapper _mapper;
        public AddStudentUserDataRepository(IMapper mapper,ApplicationDbContext context)
        {
            _context = context;
            _mapper = mapper;
        }

     
        public async Task AddStudent(AddStudentUserDataDTO add)
        {
            var addStudent = _mapper.Map<StudentUserData>(add);
            await _context.AddAsync(addStudent);
            await _context.SaveChangesAsync();
        }


    }
}
