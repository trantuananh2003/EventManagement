using System.ComponentModel.DataAnnotations;

namespace EventManagement.Models.ModelsDto.OrganizationDtos
{
    public class MemberOrganizationCreateDto
    {
        [Required]
        public string EmailUser { get; set; }
        [Required]
        public string IdOrganization { get; set; }
    }
}
