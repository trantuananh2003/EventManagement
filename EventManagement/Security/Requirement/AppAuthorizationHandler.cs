using EventManagement.Data.DataConnect;
using EventManagement.Data.Models;
using EventManagement.Middleware.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Security.Claims;

namespace EventManagement.Security.Requirement
{
    public class AppAuthorizationHandler :  IAuthorizationHandler
    {
        private readonly ApplicationDbContext _dbContext;
        private ApplicationUserManager _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AppAuthorizationHandler(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor 
            , ApplicationUserManager userManager, RoleManager<ApplicationRole> rolemanager) {
            _dbContext = dbContext;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _roleManager = rolemanager;
        }

        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            var requirement = context.PendingRequirements.ToList();
            foreach (var requirementItem in requirement)
            {
                if(requirementItem is OrganizationPermissionRequirement)
                {
                    if (IsOrganizationPermission(context.User, (OrganizationPermissionRequirement) requirementItem))
                    {
                        context.Succeed(requirementItem);
                    }
                }
            }

            return Task.CompletedTask;
        }

        private bool IsOrganizationPermission(ClaimsPrincipal user, OrganizationPermissionRequirement requirementItem)
        {
            var appUserTask = _userManager.GetUserAsync(user);
            Task.WaitAll(appUserTask);
            var appUser = appUserTask.Result;

            var idOrganization = _httpContextAccessor.HttpContext.Request.Headers["IdOrganization"].ToString();

            if (string.IsNullOrEmpty(idOrganization) || appUser == null) {
                return false;
            }

            #region Query get claim from User in Organization
            //get claim of user in organization
            var roleUserQuery = _dbContext.UserRoles.Where(ur => ur.UserId == appUser.Id); //Table UserRoles
            var roleOrganization = _dbContext.Roles.Where(r => r.OrganizationId == idOrganization); //Table Roles
                                                                                                  
            var roleUserInOrganization = from x in roleUserQuery
                                         join y in roleOrganization
                                         on x.RoleId equals y.Id into roleGroup
                                         from role in roleGroup.DefaultIfEmpty()
                                         select new
                                         {
                                             IdOrganization = role.OrganizationId,
                                             UserId = x.UserId,
                                             RoleId = x.RoleId,
                                         };
            var test = roleUserInOrganization.FirstOrDefault();
            // Lấy danh sách ClaimUser với LeftJoin
            var allClaimUserInO = from roleUser in roleUserInOrganization
                                  join roleClaim in _dbContext.RoleClaims
                                  on roleUser.RoleId equals roleClaim.RoleId into claimGroup
                                  from claim in claimGroup.DefaultIfEmpty()
                                  select new
                                  {
                                      IdOrganization = roleUser.IdOrganization,
                                      RoleId = roleUser.RoleId,
                                      UserId = roleUser.UserId,
                                      ClaimType = claim.ClaimType, // Sử dụng null-safe operator
                                      ClaimValue = claim.ClaimValue, // Tránh NullReferenceException
                                  };
            #endregion

            //Handle list claim
            var listClaimTask = allClaimUserInO.FirstOrDefaultAsync();
            listClaimTask.Wait();

            var Claim = listClaimTask.Result;
            if (requirementItem.ClaimValue != Claim.ClaimValue || requirementItem.ClaimType != Claim.ClaimType)
                return false;

            return true;
        }
    }
}
