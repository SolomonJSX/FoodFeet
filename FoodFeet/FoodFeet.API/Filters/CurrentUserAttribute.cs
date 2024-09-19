using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FoodFeet.API.Filters;

public class CurrentUserAttribute(string claimType) : ActionFilterAttribute
{
    private readonly string _claimType = claimType;

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var user = context.HttpContext.User;

        if (user.Identity?.IsAuthenticated ?? false)
        {
            var claimValue = user.FindFirstValue(_claimType);

            if (claimValue is not null)
            {
                context.ActionArguments[_claimType] = claimValue;
            }
            else
            {
                context.Result = new UnauthorizedResult();
            }
        }
        else
        {
            context.Result = new UnauthorizedResult();
        }
    }
}