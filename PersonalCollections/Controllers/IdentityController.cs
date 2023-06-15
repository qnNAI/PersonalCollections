using Domain.Entities.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PersonalCollections.Controllers {

	public class IdentityController : Controller {
		private readonly SignInManager<ApplicationUser> _signInManager;

		public IdentityController(SignInManager<ApplicationUser> signInManager)
        {
			_signInManager = signInManager;
		}

        public async Task<IActionResult> GoogleSignin() {
			var props = new AuthenticationProperties {
				RedirectUri = Url.Action("GoogleResponse")
			};
			return Challenge(props, GoogleDefaults.AuthenticationScheme);
		}

		public async Task GoogleResponse() {
			var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
			var t = externalLoginInfo.Principal;
		}
	}
}
