using EventManagement.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace EventManagement.Models.ModelsDto.OrganizationDtos
{

    public class OrganizationUpdateDto
    {
        [Required]
        public string IdOrganization { get; set; }
        [Required]
        public string NameOrganization { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public IFormFile? File { get; set; }
    }
}
