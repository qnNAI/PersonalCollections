using System;
using System.Collections.Generic;
using System.Linq;
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

            if(user.ThirdPartyAuth) {
                return SignInResult.Failed;
            }

            return await base.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
        }

        public override async Task SignInAsync(ApplicationUser user, bool isPersistent, string? authenticationMethod = null) {


            await base.SignInAsync(user, isPersistent, authenticationMethod);
        }

        protected override Task<SignInResult?> PreSignInCheck(ApplicationUser user) {
            return base.PreSignInCheck(user);
        }
    }
}
