using System.ComponentModel.DataAnnotations;

namespace RecordManagementSystem.DTO.Account
{
    public class LoginApiDTO
    {

        [EmailAddress(ErrorMessage = "Email address format is not valid please enter your email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required please enter your Password")]
        public string Password { get; set; }
    }
}
