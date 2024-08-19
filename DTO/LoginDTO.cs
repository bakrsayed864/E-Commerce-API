using System.ComponentModel.DataAnnotations;

namespace Own_Service.DTO
{
    public class LoginDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
