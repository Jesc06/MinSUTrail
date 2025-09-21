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
using RecordManagementSystem.Domain.Entities.OTP;
using Azure;
using RecordManagementSystem.Application.Features.OTP.Services;
using RecordManagementSystem.Application.Features.OTP.DTO;
using RecordManagementSystem.Application.Features.OTP.Interfaces;

namespace RecordManagementSystem.Infrastructure.Repository.Features.Account
{
    public class AddStudentUserAccountRepository : IAddStudentUserData 
    {

        private readonly ApplicationDbContext _context;
        private IEmailService _otpEmailService;
        private Random otpGenerator = new Random();
        public AddStudentUserAccountRepository(ApplicationDbContext context, IEmailService otpEmailService)
        {
            _context = context;
            _otpEmailService = otpEmailService;
        }

        public async Task<OTPRequest> AddStudentAccount(OTPRequestDTO addStudentDTO)
        {
            //OTP generate
            var sessionId = Guid.NewGuid();
            var otp = otpGenerator.Next();

            var addStudentAccount = new OTPRequest
            {
                SessionID = sessionId.ToString(),
                OTP = otp,
                Email = addStudentDTO.Email,
                ExpiryTime = DateTime.UtcNow,

                FirstName = addStudentDTO.FirstName,
                Middlename = addStudentDTO.Middlename,
                LastName = addStudentDTO.LastName,
                Gender = addStudentDTO.Gender,
                YearOfBirth = addStudentDTO.YearOfBirth,
                MonthOfBirth = addStudentDTO.MonthOfBirth,
                DateOfBirth = addStudentDTO.DateOfBirth,
                HomeAddress = addStudentDTO.HomeAddress,
                MobileNumber = addStudentDTO.MobileNumber,
                Program = addStudentDTO.Program,
                YearLevel = addStudentDTO.YearLevel,
                StudentID = addStudentDTO.StudentID,
                Password = addStudentDTO.Password
            };

            await _context.OTPRequests.AddAsync(addStudentAccount);
            await _context.SaveChangesAsync();

            await _otpEmailService.SendEmailAsync(addStudentDTO.Email, "OTP", otp);
            
            return addStudentAccount;
        }


        public async Task<bool> VerifyOTP(VerifyOTPDTO verifyOTP)
        {
            var OTPentry = await _context.OTPRequests.FirstOrDefaultAsync(x => x.SessionID == verifyOTP.sessionID);
            if (OTPentry == null)
                return false;

            if((DateTime.UtcNow - OTPentry.ExpiryTime).TotalSeconds > 40)
            {
                _context.OTPRequests.Remove(OTPentry);
                await _context.SaveChangesAsync();
                return false;
            }

            if (OTPentry.OTP != verifyOTP.OTP)
                return false;


            var student = new StudentUserAccount {
                FirstName = OTPentry.FirstName,
                Middlename = OTPentry.Middlename,
                LastName = OTPentry.LastName,
                Gender = OTPentry.Gender,
                YearOfBirth = OTPentry.YearOfBirth,
                MonthOfBirth = OTPentry.MonthOfBirth,
                DateOfBirth = OTPentry.DateOfBirth,
                HomeAddress = OTPentry.HomeAddress,
                MobileNumber = OTPentry.MobileNumber,
                Email = OTPentry.Email,
                Program = OTPentry.Program,
                YearLevel = OTPentry.YearLevel,
                StudentID = OTPentry.StudentID,
                Password = OTPentry.Password
            };


            _context.studentUserAccount.Add(student);
            _context.OTPRequests.Remove(OTPentry);
            await _context.SaveChangesAsync();

            return true;


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
