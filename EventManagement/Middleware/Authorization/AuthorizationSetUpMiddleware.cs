using EventManagement.Common;
using EventManagement.Middleware.AuthorizationSetUp.Requirement;
using Microsoft.AspNetCore.Authorization;

namespace EventManagement.Middleware.AuthorizationSetUp
{
    public static class AuthorizationSetUpMiddleware
    {
        public static IServiceCollection AddSetupAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(opts =>
            {
                //Event
                opts.AddPolicy(SD_Role_Permission.AddEvent_ClaimValue, policy =>
                {
                    policy.Requirements.Add(new OrganizationPermissionRequirement("Organization", SD_Role_Permission.AddEvent_ClaimValue));
                });
                opts.AddPolicy(SD_Role_Permission.UpdateEvent_ClaimValue, policy =>
                {
                    policy.Requirements.Add(new OrganizationPermissionRequirement("Organization", SD_Role_Permission.UpdateEvent_ClaimValue));
                });
                opts.AddPolicy(SD_Role_Permission.ViewReportEvent_ClaimValue, policy =>
                {
                    policy.Requirements.Add(new OrganizationPermissionRequirement("Organization", SD_Role_Permission.ViewReportEvent_ClaimValue));
                });
                opts.AddPolicy(SD_Role_Permission.ManageOrderOverView, policy =>
                {
                    policy.Requirements.Add(new OrganizationPermissionRequirement("Organization", SD_Role_Permission.ManageOrderOverView));
                });


                //Suport chat
                opts.AddPolicy(SD_Role_Permission.SupportChat_ClaimValue, policy =>
                {
                    policy.Requirements.Add(new OrganizationPermissionRequirement("Organization", SD_Role_Permission.SupportChat_ClaimValue));
                });

                //Organzation - Manage team
                opts.AddPolicy(SD_Role_Permission.ManageOrganization_ClaimValue, policy =>
                {
                    policy.Requirements.Add(new OrganizationPermissionRequirement("Organization", SD_Role_Permission.ManageOrganization_ClaimValue));
                });
                opts.AddPolicy(SD_Role_Permission.ManageMember_ClaimValue, policy =>
                {
                    policy.Requirements.Add(new OrganizationPermissionRequirement("Organization", SD_Role_Permission.ManageMember_ClaimValue));
                });
                opts.AddPolicy(SD_Role_Permission.ManageRole_ClaimValue, policy =>
                {
                    policy.Requirements.Add(new OrganizationPermissionRequirement("Organization", SD_Role_Permission.ManageRole_ClaimValue));
                });
            });


            services.AddTransient<IAuthorizationHandler, AppAuthorizationHandler>();
            return services;
        }
    }
}
