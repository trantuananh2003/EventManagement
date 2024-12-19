using EventManagement.Middleware.AuthorizationSetUp;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EventManagement.Middleware
{
    public static class ConfigurationJWT
    {
        public static void SetUpJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var key = configuration.GetValue<string>("ApiSetting:Secret");
            services.AddAuthentication(u =>
            {
                u.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                u.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(u =>
            {
                u.RequireHttpsMetadata = false;
                u.SaveToken = true;
                u.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    ValidateAudience = false,
                    ValidateIssuer = false,
                };
            });
        }
    }
}
