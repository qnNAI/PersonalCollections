using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;

namespace PersonalCollections.Controllers {

	public class IdentityController : Controller {

		[HttpGet]
		public async Task Signin() {
			await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties {
				RedirectUri = Url.Action("GoogleSignin")
			});
		}

		public async Task GoogleSignin() {

		}
	}
}
