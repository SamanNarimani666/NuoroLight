﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace NuoroLight.Web.HttpContext;
public class RedirectIfLoggedInActionFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        if (context.HttpContext.User.Identity.IsAuthenticated)
        {
            if (context.Controller is Controller controller)
            {
                context.HttpContext.Response.Redirect("/");
            }
        }
    }
}

