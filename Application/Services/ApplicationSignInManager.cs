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
			: base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation) 
		{
			this._userManager = userManager;
		}


	}
}
