using System.ComponentModel.DataAnnotations;

namespace EventManagement.Models.ModelsDto
{
    public class LoginRequestDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
