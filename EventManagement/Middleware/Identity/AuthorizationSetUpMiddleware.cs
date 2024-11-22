using EventManagement.Security.Requirement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace EventManagement.Middleware.Identity
{
    public static class AuthorizationSetUpMiddleware
    {
        public static IServiceCollection AddSetupAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(opts =>
            {
                opts.AddPolicy("AddEventPolicy", policy =>
                {
                    policy.Requirements.Add(new OrganizationPermissionRequirement("Organization", "AddEvent"));
                });
            });


            services.AddTransient<IAuthorizationHandler, AppAuthorizationHandler>();

            return services;
        }
    }
}
