namespace EventManagement.Models.ModelsDto.OrganizationDtos
{
    public class MemberOrganizationDto
    {
        public string MemberId { get; set; }
        public UserOrganizationDto User { get; set; }
    }

    public class UserOrganizationDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
    }
}
