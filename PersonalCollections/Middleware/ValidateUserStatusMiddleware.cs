using System.Security.Claims;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace PersonalCollections.Middleware
{

    public class ValidateUserStatusMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidateUserStatusMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<ValidateUserStatusMiddleware> logger
            )
        {
            var userPrincipal = context.User;

            var username = userPrincipal.Identity.Name;

            if(username is null)
            {
                await _next(context);
                return;
            }

            var user = await userManager.FindByNameAsync(username);

            if(user is null || !user.IsActive)
            {
                await context.ForbidAsync();
                await signInManager.SignOutAsync();
                if (user is not null)
                {
                    logger.LogWarning("Unauthorized attempt to access resourses: inactive user - {userId}", user.Id);
                }
                return;
            }

            await _next(context);
        }
    }

    public static class ValidateUserStatusMiddlewareExtensions
    {

        public static IApplicationBuilder UseUserStatusValidation(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ValidateUserStatusMiddleware>();
        }
    }
}

