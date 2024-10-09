using EventManagement.Data.Models;

namespace EventManagement.Models.ModelsDto
{
    public class OrganizationRequest
    {
        public string NameOrganization { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
