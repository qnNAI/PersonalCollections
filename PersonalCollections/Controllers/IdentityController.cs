﻿using System.Security.Claims;
using Application.Common.Contracts;
using Application.Common.Contracts.Services;
using Application.Models.Identity;
using AspNet.Security.OAuth.GitHub;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(SignInRequest request) {
            if(!ModelState.IsValid) {
                return View(request);
            }

            var result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, isPersistent: true, lockoutOnFailure: false);
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
        [AllowAnonymous]
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
			var props = new AuthenticationProperties {
				RedirectUri = Url.Action("GoogleResponse")
			};
			return Challenge(props, GoogleDefaults.AuthenticationScheme);
		}

		public async Task<IActionResult> GoogleResponse() {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if(result is null || result.Principal is null) {
                return RedirectToAction("Error", new[] {
                    new IdentityError {
                        Code = "GoogleAuthFailed",
                        Description = "Failed to authenticate with Google"
                    }
                });
            }

            return await _SignInExternalAsync(result.Principal);
        }

        public IActionResult GithubSignIn() {
            var props = new AuthenticationProperties {
                RedirectUri = Url.Action("GithubResponse")
            };
            return Challenge(props, GitHubAuthenticationDefaults.AuthenticationScheme);
        }

		public async Task<IActionResult> GithubResponse() {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if(result is null || result.Principal is null) {
                return RedirectToAction("Error", new[] {
                    new IdentityError {
                        Code = "GithubAuthFailed",
                        Description = "Failed to authenticate with Github"
                    }
                });
            }

            return await _SignInExternalAsync(result.Principal);
        }

		[HttpPost]
		[Authorize]
		public new async Task<IActionResult> SignOut() {
			await _signInManager.SignOutAsync();
            return Redirect("/");
        }

        private async Task<IActionResult> _SignInExternalAsync(ClaimsPrincipal principal) {
            var userIdentifier = principal.FindFirstValue(ClaimTypes.NameIdentifier);

            if(userIdentifier is null) {
                return RedirectToAction("Error", new[] {
                    new IdentityError {
                        Code = "ExternalAuthFailed",
                        Description = "External authentication failed. User identifier not found!"
                    }
                });
            }

            var user = await _userManager.FindByIdAsync(userIdentifier);

            if(user is null) {
                var signUpResult = await _identityService.SignUpExternalAsync(new SignUpExternalRequest {
                    Id = userIdentifier,
                    Email = principal.FindFirst(ClaimTypes.Email)?.Value,
                    Username = principal.Identity!.Name!
                });
                if(!signUpResult.Succeeded) {
                    return View("Error", signUpResult.Errors);
                }
                user = await _userManager.FindByIdAsync(userIdentifier);
            }

            await _signInManager.SignInAsync(user!, true);
            return Redirect("/");
        }

        private void _AddModelErrors(IdentityResult result) {
            foreach(var error in result.Errors ?? Enumerable.Empty<IdentityError>()) {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}
