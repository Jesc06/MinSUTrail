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





    }
}
