using System.Security.Claims;
using Application.Common.Contracts;
using Application.Common.Contracts.Services;
using Application.Models.Identity;
using AspNet.Security.OAuth.GitHub;
using Domain.Entities.Identity;
using Mapster;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInRequest request) {
            if(!ModelState.IsValid) {
                return View(request);
            }

            var result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, isPersistent: true, lockoutOnFailure: false);

            if (result.IsNotAllowed) {
                ModelState.AddModelError("", "Authentication failed. User is locked!");
                return View(request);
            }

            if(!result.Succeeded) {
                ModelState.AddModelError("", "Authentication failed!");
                return View(request);
            }

            return Redirect("/");
        }

        [HttpGet]
        public IActionResult SignUp() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpRequest request) {
            var result = await _identityService.SignUpAsync(request);
            if(!result.Succeeded) {
                ModelState.AddModelError("", "Registration failed!");
                _AddModelErrors(result);
                return View(request);
            }

            return RedirectToAction("Signin");
        }

        public IActionResult GoogleSignIn() {
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(GoogleDefaults.AuthenticationScheme, Url.Action("ExternalResponse"));
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
		}

        public IActionResult GitHubSignIn() {
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(GitHubAuthenticationDefaults.AuthenticationScheme, Url.Action("ExternalResponse"));
            return Challenge(properties, GitHubAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> ExternalResponse() {
            var info = await _signInManager.GetExternalLoginInfoAsync();

            if(info is null || info.Principal is null) {
                return View("Error", new[] {
                    new IdentityError {
                        Code = "ExternalAuthFailed",
                        Description = "Failed to authenticate with External service"
                    }
                });
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if(signInResult.Succeeded) {
                return Redirect("/");
            }

            if (signInResult.IsNotAllowed) {
                return View("Error", new[] {
                    new IdentityError {
                        Code = "ExternalAuthFailed",
                        Description = "Failed to authenticate: user is locked!"
                    }
                });
            }

            ViewData["Provider"] = info.LoginProvider;
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            return View("ExternalLogin", new ExternalLoginModel { Email = email });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLogin(ExternalLoginModel model) {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if(info == null) {
                ModelState.AddModelError("", "Failed to authenticate with external service!");
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email!);
            IdentityResult result;

            if(user != null) {
                ModelState.AddModelError("", "Failed to authenticate with external service. Email already taken!");
                return View(model);
            } else {
                user = new ApplicationUser {
                    Email = model.Email,
                    UserName = info.Principal.Identity.Name,
                    IsActive = true,
                    RegistrationDate = DateTime.UtcNow
                };
                result = await _userManager.CreateAsync(user);
                if(result.Succeeded) {
                    var addToRoleResult = await _userManager.AddToRoleAsync(user, "User");
                    result = await _userManager.AddLoginAsync(user, info);
                    if(result.Succeeded && addToRoleResult.Succeeded) {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return Redirect("/");
                    }
                }
            }

            foreach(var error in result.Errors) {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }

        [HttpPost]
		[Authorize]
		public new async Task<IActionResult> SignOut() {
			await _signInManager.SignOutAsync();
            return Redirect("/");
        }

        public IActionResult AccessDenied() {
            return View();
        }

        private void _AddModelErrors(IdentityResult result) {
            foreach(var error in result.Errors ?? Enumerable.Empty<IdentityError>()) {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}
