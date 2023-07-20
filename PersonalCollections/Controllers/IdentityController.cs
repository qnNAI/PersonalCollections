using System.Security.Claims;
using Application.Common.Contracts;
using Application.Common.Contracts.Services;
using Application.Models.Email;
using Application.Models.Identity;
using AspNet.Security.OAuth.GitHub;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace PersonalCollections.Controllers {

	public class IdentityController : Controller {
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;
        private readonly IIdentityService _identityService;
        private readonly IEmailService _emailService;
        private readonly IStringLocalizer<IdentityController> _localizer;

        public IdentityController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IIdentityService identityService,
            IEmailService emailService,
            IStringLocalizer<IdentityController> localizer)
        {
			_signInManager = signInManager;
			_userManager = userManager;
            _identityService = identityService;
            _emailService = emailService;
            _localizer = localizer;
        }

        #region Authentication

        [HttpGet]
		public IActionResult SignIn() {
			return View();
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInRequest request, string returnUrl) {
            if(!ModelState.IsValid) {
                return View(request);
            }

            var existing = await _userManager.FindByEmailAsync(request.Login);
            var result = await _signInManager.PasswordSignInAsync(existing is null ? request.Login : existing.UserName!, request.Password, isPersistent: true, lockoutOnFailure: false);

            if (result.IsNotAllowed) {
                ModelState.AddModelError("", _localizer["UserLocked"].Value);
                return View(request);
            }

            if(!result.Succeeded) {
                ModelState.AddModelError("", _localizer["AuthFailed"].Value);
                return View(request);
            }

            
            return string.IsNullOrEmpty(returnUrl) ? Redirect("/") : Redirect(returnUrl);
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
                ModelState.AddModelError("", _localizer["SignUpFailed"]);
                _AddModelErrors(result);
                return View(request);
            }

            return RedirectToAction("Signin");
        }

        [HttpPost]
        [Authorize]
        public new async Task<IActionResult> SignOut() {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }

        [HttpGet]
        public IActionResult AccessDenied() {
            return View();
        }

        #endregion

        #region External auth

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
                        Description = _localizer["ExternalAuthFailed"].Value
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
                        Description = _localizer["UserLocked"].Value
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
                ModelState.AddModelError("", _localizer["ExternalAuthFailed"].Value);
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email!);
            IdentityResult result;

            if(user != null) {
                ModelState.AddModelError("", _localizer["AuthFailedEmailTaken"].Value);
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

        #endregion

        #region Password recovery

        [HttpGet]
        public IActionResult ForgotPassword() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request) {
            if (!ModelState.IsValid) {
                return View(request);
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null) {
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callback = Url.Action(nameof(ResetPassword), "Identity", new { token, email = user.Email }, Request.Scheme);

            var message = new Message(new string[] { user.Email }, "Reset password link", callback);
            await _emailService.SendEmailAsync(message);

            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation() {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email) {
            var request = new ResetPasswordRequest { Email = email, Token = token };
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request) {
            if (!ModelState.IsValid) {
                return View(request);
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null) {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            var resetResult = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
            if (!resetResult.Succeeded) {
                foreach(var error in resetResult.Errors) {
                    ModelState.AddModelError("", error.Description);
                }

                return View();
            }

            return RedirectToAction(nameof(ResetPasswordConfirmation));
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation() {
            return View();
        }

        #endregion

        #region User management

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult UserManagement() {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Users(int page = 1, int pageSize = 10, CancellationToken cancellationToken = default) {
            var users = await _identityService.GetUsersAsync(page, pageSize, cancellationToken);

            ViewData["page"] = page;
            ViewData["total"] = (int)Math.Ceiling((double)(await _identityService.CountUsersAsync(cancellationToken)) / pageSize);

            return PartialView("_UsersPartial", users);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUsers(string[] users, CancellationToken cancellationToken) {
            await _identityService.DeleteMultipleAsync(users, cancellationToken);

            if(_IsCurrentUserAffected(users)) {
                await _signInManager.SignOutAsync();
            }

            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> LockUsers(string[] users, CancellationToken cancellationToken) {
            await _identityService.LockMultipleAsync(users, cancellationToken);

            if(_IsCurrentUserAffected(users)) {
                await _signInManager.SignOutAsync();
            }

            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnlockUsers(string[] users, CancellationToken cancellationToken) {
            await _identityService.UnlockMultipleAsync(users, cancellationToken);
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminAdd(string[] users, CancellationToken cancellationToken) {
            await _identityService.AddAdminMultipleAsync(users, cancellationToken);
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminRemove(string[] users, CancellationToken cancellationToken) {
            await _identityService.RemoveAdminMultipleAsync(users, cancellationToken);

            if(_IsCurrentUserAffected(users)) {
                await _signInManager.SignOutAsync();
            }

            return Ok();
        }

        private bool _IsCurrentUserAffected(string[] users) {
            return users.Contains(HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
        }

        #endregion

        private void _AddModelErrors(IdentityResult result) {
            foreach(var error in result.Errors ?? Enumerable.Empty<IdentityError>()) {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}
