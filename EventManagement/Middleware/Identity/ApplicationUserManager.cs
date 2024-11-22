using EventManagement.Data.DataConnect;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using EventManagement.Data.Models;

namespace EventManagement.Middleware.Identity
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        private readonly UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, string, IdentityUserClaim<string>,
            IdentityUserRole<string>, IdentityUserLogin<string>, IdentityUserToken<string>, IdentityRoleClaim<string>> _store;

        public ApplicationUserManager(
            IUserStore<ApplicationUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IEnumerable<IUserValidator<ApplicationUser>> userValidators,
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<ApplicationUser>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _store = (UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, string, IdentityUserClaim<string>,
                IdentityUserRole<string>, IdentityUserLogin<string>, IdentityUserToken<string>, IdentityRoleClaim<string>>)store;
        }

        public virtual async Task<IdentityResult> AddToRoleByRoleIdAsync(ApplicationUser user, string roleId)
        {
            ThrowIfDisposed();

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrWhiteSpace(roleId))
                throw new ArgumentNullException(nameof(roleId));

            if (await IsInRoleByRoleIdAsync(user, roleId, CancellationToken))
                return IdentityResult.Failed(ErrorDescriber.UserAlreadyInRole(roleId));

            _store.Context.Set<IdentityUserRole<string>>().Add(new IdentityUserRole<string> { RoleId = roleId, UserId = user.Id });

            return await UpdateUserAsync(user);
        }

        public async Task<bool> IsInRoleByRoleIdAsync(ApplicationUser user, string roleId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrWhiteSpace(roleId))
                throw new ArgumentNullException(nameof(roleId));

            var role = await _store.Context.Set<ApplicationRole>().FindAsync(roleId);
            if (role == null)
                return false;

            var userRole = await _store.Context.Set<IdentityUserRole<string>>().FindAsync(new object[] { user.Id, roleId }, cancellationToken);
            return userRole != null;
        }
    }
}
