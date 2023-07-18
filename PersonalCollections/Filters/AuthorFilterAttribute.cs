using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PersonalCollections.Filters
{
    public class AuthorFilterAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controller = context.Controller as Controller;
            var userId = context.HttpContext.Request.Query["userId"].ToString();

            if (userId is null)
            {
                Forbid(context, controller);
                return;
            }

            var currentUser = context.HttpContext.User;
            if (currentUser is null)
            {
                Forbid(context, controller);
                return;
            }

            if (currentUser?.FindFirstValue(ClaimTypes.NameIdentifier) != userId && !currentUser.IsInRole("Admin"))
            {
                Forbid(context, controller);
                return;
            }

            await base.OnActionExecutionAsync(context, next);
        }

        private void Forbid(ActionExecutingContext context, Controller? controller)
        {
            context.Result = controller.Forbid();
            var logger = context.HttpContext.RequestServices.GetService<ILogger<AuthorFilterAttribute>>();
            logger.LogWarning("Unauthorized attempt to access author resourses: {userId}", context.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}
