using Microsoft.AspNetCore.Authorization;

namespace EventManagement.Middleware.AuthorizationSetUp.Requirement
{
    public class OrganizationPermissionRequirement : IAuthorizationRequirement
    {
        public string ClaimType { get; }
        public string ClaimValue { get; }
        public OrganizationPermissionRequirement(string claimType, string claimValue)
        {
            ClaimType = claimType;
            ClaimValue = claimValue;
        }
    }
}
