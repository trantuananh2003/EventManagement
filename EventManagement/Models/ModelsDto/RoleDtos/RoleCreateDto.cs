﻿namespace EventManagement.Models.ModelsDto.RoleDtos
{
    public class RoleCreateDto
    {
        public string NameRole { get; set; }
        public string OrganizationId { get; set; }
        public string Description { get; set; }
        public string[] ClaimValues { get; set; }
    }
}
