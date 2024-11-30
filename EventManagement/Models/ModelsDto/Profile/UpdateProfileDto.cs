namespace EventManagement.Models.ModelsDto.Profile
{
    public class UpdateProfileDto
    {
        public string FullName { get; set; }
        public IFormFile? File { get; set; }
    }
}
