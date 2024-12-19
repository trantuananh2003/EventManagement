using System.ComponentModel.DataAnnotations;

namespace EventManagement.Models.ModelsDto.Profile
{
    public class UpdateProfileDto
    {
        [Required]
        public string FullName { get; set; }
        public IFormFile? File { get; set; }
    }
}
