using System.ComponentModel.DataAnnotations;

namespace EventManagement.Models.ModelsDto.OrganizationDtos
{
    public class OrganizationCreateDto
    {
        [Required]
        public string IdUserOwner { get; set; }
        [Required]
        public string NameOrganization { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }

}
