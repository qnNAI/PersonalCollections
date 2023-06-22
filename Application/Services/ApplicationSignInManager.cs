using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.Services {
    public class ApplicationSignInManager : SignInManager<ApplicationUser> {
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationSignInManager(UserManager<ApplicationUser> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<ApplicationUser>> logger,
            IAuthenticationSchemeProvider schemes,
            IUserConfirmation<ApplicationUser> confirmation)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation) {

            this._userManager = userManager;
        }

        public override async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure) {

            var user = await _userManager.FindByNameAsync(userName);
            if(user is null) {
                return SignInResult.Failed;
            }

            if(!user.IsActive) {
                return SignInResult.NotAllowed;
            }

            return await base.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
        }

        public override async Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent, bool bypassTwoFactor) {
            var info = await GetExternalLoginInfoAsync();
            if (info is not null) {
                var user = await _userManager.FindByEmailAsync(info.Principal.FindFirstValue(ClaimTypes.Email) ?? string.Empty);
                if (user is not null && !user.IsActive) {
                    return SignInResult.NotAllowed;
                }
            }

            return await base.ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent, bypassTwoFactor);
        }

        public override async Task<ClaimsPrincipal> CreateUserPrincipalAsync(ApplicationUser user) {
            var principal = await base.CreateUserPrincipalAsync(user);

            var roles = await _userManager.GetRolesAsync(user);
            foreach(var role in roles) {
                principal.Identities.FirstOrDefault().AddClaim(new Claim(ClaimTypes.Role, role));
            }

            return principal;
        }


    }
}
