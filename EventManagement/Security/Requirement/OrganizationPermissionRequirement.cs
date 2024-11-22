using Microsoft.AspNetCore.Authorization;

namespace EventManagement.Security.Requirement
{
    public class OrganizationPermissionRequirement : IAuthorizationRequirement
    {
        public string ClaimType { get; }
        public string ClaimValue { get; }
        public OrganizationPermissionRequirement(string claimType, string claimValue)
        {
            this.ClaimType = claimType;
            this.ClaimValue = claimValue;
        }
    }
}
