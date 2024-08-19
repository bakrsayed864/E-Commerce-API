using System;
using System.ComponentModel.DataAnnotations;

namespace Own_Service.DTO
{
    public class RegisterUserDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public string Job { get; set; }
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

      
    }
}
