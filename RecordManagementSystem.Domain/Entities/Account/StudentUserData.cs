using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordManagementSystem.Domain.Entities.Account
{
    public class StudentUserData
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Middlename { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string YearOfBirth { get; set; }
        public string MonthOfBirth { get; set; }
        public string DateOfBirth { get; set; }
        public string HomeAddress { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Program { get; set; }
        public string YearLevel { get; set; }
        public string StudentID { get; set; }
        public string Password { get; set; }
    }
}
