using System.Security.Claims;
using Application.Common.Contracts;
using Application.Common.Contracts.Services;
using Application.Models.Identity;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PersonalCollections.Controllers {

	public class IdentityController : Controller {
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;
        private readonly IIdentityService _identityService;

        public IdentityController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IIdentityService identityService)
        {
			_signInManager = signInManager;
			_userManager = userManager;
            _identityService = identityService;
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

		public async Task<IActionResult> GoogleResponse() {
			var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			if (result is null || result.Principal is null) {
				return RedirectToAction("Error", new[] {
					new IdentityError {
						Code = "GoogleAuthFailed",
						Description = "Failed to authenticate with Google"
					}
				});
			}

			var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
			var username = result.Principal.Identity.Name;
            var user = await _userManager.FindByEmailAsync(email);

			if (user == null) {
				var signUpResult = await _identityService.SignUpExternalAsync(new SignUpExternalRequest { 
					Email = email,
					Username = username
                });
				if(!signUpResult.Succeeded) {
					return RedirectToAction("Error", signUpResult.Errors);
				}
                user = await _userManager.FindByEmailAsync(email);
            }
			
			await _signInManager.SignInAsync(user, true);
			return Redirect("/");
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Signout() {
			await _signInManager.SignOutAsync();
            return Redirect("/");
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

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error(IEnumerable<IdentityError> errors) {
			return View();
		}
	}
}
