using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecordManagementSystem.Application.Features.Account.DTO;
using RecordManagementSystem.Application.Features.Account.Interface;
using RecordManagementSystem.Infrastructure.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordManagementSystem.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<UserIdentity> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserIdentity> _userManager;    
        public AuthService(SignInManager<UserIdentity> signInManager, UserManager<UserIdentity> userManager, ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }


        public async Task<bool> RegisterStudentAccount(RegisterStudentAccountDTO registerAccount)
        {
            UserIdentity userData = new UserIdentity
            {
                FirstName = registerAccount.Firtsname,
                MiddleName = registerAccount.MiddleName,
                LastName = registerAccount.LastName,
                Email = registerAccount.Email,
                UserName = registerAccount.Email
            };
            var register = await _userManager.CreateAsync(userData, registerAccount.Password);
            if (register.Succeeded)
            {
                var roles = await _userManager.AddToRoleAsync(userData, "Student");
                var find = _context.studentUserAccount.Find(registerAccount.Id);
                if (find is not null)
                {
                    var suc = _context.studentUserAccount.Remove(find);
                    _context.SaveChanges();
                }
                return true;
            }
            return false;
        }


        public async Task<bool> Login(LoginDTO loginDTO)
        {
            var isLogin = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, true, true);
            if (isLogin.Succeeded)
            {
                return true;
            }
            return false;
        }


        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }




    }
}
