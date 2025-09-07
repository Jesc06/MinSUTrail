using System.ComponentModel.DataAnnotations;

namespace RecordManagementSystem.DTOs.Account
{
    public class RegisterDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Firstname is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Middlename is required")]
        public string Middlename { get; set; }

        [Required(ErrorMessage = "Lastname is required")]
        public string LastName { get; set; }
 
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
       
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
