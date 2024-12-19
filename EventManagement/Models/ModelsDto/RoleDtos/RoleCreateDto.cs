using System.ComponentModel.DataAnnotations;

namespace EventManagement.Models.ModelsDto.RoleDtos
{
    public class RoleCreateDto
    {
        [Required]
        public string NameRole { get; set; }
        public string OrganizationId { get; set; }
        public string Description { get; set; }
        public string[] ClaimValues { get; set; }
    }
}
