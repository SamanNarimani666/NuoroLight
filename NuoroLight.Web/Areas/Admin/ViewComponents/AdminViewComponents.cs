using Microsoft.AspNetCore.Mvc;
using NuoroLight.Application.Services.Interfaces;
using NuoroLight.Web.PresentationExtensions;
namespace NuoroLight.Web.Areas.Admin.ViewComponents;

#region AdminUserInformation
public class AdminUserInformation : ViewComponent
{
    private readonly IUserService _userService;

    public AdminUserInformation(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View("AdminUserInformation", await _userService.GetUserInformation(User.GetUserId()));
    }
}
#endregion

#region SidebarMenu
public class SidebarMenu : ViewComponent
{
    [ResponseCache(Location = ResponseCacheLocation.Any, NoStore = true)]
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View("SidebarMenu");
    }
}
#endregion


