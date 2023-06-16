using Application.Common.Contracts;
using Application.Models.Identity;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PersonalCollections.Controllers {

	public class IdentityController : Controller {
		private readonly SignInManager<ApplicationUser> _signInManager;

		public IdentityController(SignInManager<ApplicationUser> signInManager)
        {
			_signInManager = signInManager;
		}

		[HttpGet]
		public IActionResult Signin() {
			return View();
		}

        public async Task<IActionResult> GoogleSignin() {
			var props = new AuthenticationProperties {
				RedirectUri = Url.Action("GoogleResponse")
			};
			return Challenge(props, GoogleDefaults.AuthenticationScheme);
		}

		public async Task GoogleResponse() {
			var t = HttpContext;
			//var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
			//var t = externalLoginInfo.Principal;
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Signout() {
			await HttpContext.SignOutAsync();
            return RedirectToAction("SignIn");
        }

        [HttpGet]
        public IActionResult SignUp() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpRequest request) {
            //var result = await _identityService.SignUpAsync(request);
            //if(!result.Succeeded) {
            //    ModelState.AddModelError("", "Registration failed!");
            //    _AddModelErrors(result);
            //    return View(request);
            //}

            //await _SignInAsync(result);
            return RedirectToAction("");
        }
    }
}
