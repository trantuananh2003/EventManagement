using EventManagement.Data.DataConnect;
using EventManagement.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Middleware.Identity
{
    public static class ConfigurationIdentity
    {
        public static void SetUpIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddUserManager<ApplicationUserManager>()
                .AddRoleValidator<RoleValidationCustom>();

            var defaultRoleValidator = services.FirstOrDefault(descriptor =>
                descriptor.ImplementationType == typeof(RoleValidator<ApplicationRole>));
            if (defaultRoleValidator != null)
            {
                services.Remove(defaultRoleValidator);
            }

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.Lockout.AllowedForNewUsers = false; // Tắt lockout cho người dùng mới
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(0); // Không giới hạn thời gian khóa
                options.Lockout.MaxFailedAccessAttempts = int.MaxValue; // Không giới hạn số lần thử
            });
        }
    }
}
