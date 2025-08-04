using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordManagementSystem.Application.Features.Account.DTO
{
    public class RegisterStudentAccountDTO
    {
        public string Firtsname { get; set; }   
        public string MiddleName { get; set; }  
        public string LastName { get; set; }
        public string Email { get; set; }   
        public string Password { get; set; }
    }
}
