using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NuoroLight.Application.Services.Interfaces;
using NuoroLight.Web.PresentationExtensions;
namespace NuoroLight.Web.HttpContext;
public class PermissionCheckerAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    private int _permissionId;
    private IPermissionService _permissionService;
    public PermissionCheckerAttribute(int permissionId)
    {
        _permissionId = permissionId;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        _permissionService = (IPermissionService)context.HttpContext.RequestServices.GetService(typeof(IPermissionService));
        if (context.HttpContext.User.Identity.IsAuthenticated)
        {
            var userId = context.HttpContext.User.GetUserId();
            if (!_permissionService.CheckUserPermission(userId, _permissionId).Result)
            {
                context.Result = new RedirectResult("/Dashboard");
            }
        }
        else
        {
            context.Result = new RedirectResult("/Login");
        }
    }
}

