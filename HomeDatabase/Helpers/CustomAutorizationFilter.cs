using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HomeDatabase.Database
{

    public class CustomAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                // User is not authenticated, redirect to the unauthorized view
                context.Result = new RedirectToActionResult("Unauthorized", "Home", null);
            }
        }

    }
}
