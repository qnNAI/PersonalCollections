using System.Security.Claims;
using Application.Common.Contracts.Contexts;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace PersonalCollections.CookieValidators;


public static class ActiveUserValidator {

    public static async Task ValidateAsync(CookieValidatePrincipalContext context) {
        var userPrincipal = context.Principal;
        var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();

        var userId = userPrincipal.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
        var user = await userManager.FindByIdAsync(userId);

        if(user is null || !user.IsActive) {
            context.RejectPrincipal();
            await context.HttpContext.SignOutAsync();
        }
    }
}
