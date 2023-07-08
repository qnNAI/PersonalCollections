using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PersonalCollections.Filters
{
    public class PersonalInfoFilterAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controller = context.Controller as Controller;
            var userId = context.HttpContext.Request.Query["userId"].ToString();

            if (userId is null)
            {
                context.Result = controller.Forbid();
                return;
            }

            var currentUser = context.HttpContext.User;
            if (currentUser is null)
            {
                context.Result = controller.Forbid();
                return;
            }

            if (currentUser?.FindFirstValue(ClaimTypes.NameIdentifier) != userId && !currentUser.IsInRole("Admin"))
            {
                context.Result = controller.Forbid();
                return;
            }

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
