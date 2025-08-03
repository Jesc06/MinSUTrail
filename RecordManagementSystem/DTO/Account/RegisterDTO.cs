﻿using System.ComponentModel.DataAnnotations;

namespace RecordManagementSystem.DTO.Account
{
    public class RegisterDTO
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]  
        public string Password { get; set; }
    }
}
