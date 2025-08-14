using System.ComponentModel.DataAnnotations;

namespace RecordManagementSystem.DTO.Account
{
    public class RegisterDTO
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "FirstName is required please enter your FirstName")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "MiddleName is required please enter your MiddleName")]
        public string MiddleName { get; set; }


        [Required(ErrorMessage = "LastName is required please enter your LastName")]
        public string LastName { get; set; }


        [EmailAddress(ErrorMessage = "Email address format is not valid please enter your email.")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required please enter your Password")]
        public string Password { get; set; }
    }
}
