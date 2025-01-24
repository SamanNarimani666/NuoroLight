using Microsoft.AspNetCore.Mvc;
using NuoroLight.Application.Services.Interfaces;
namespace NuoroLight.Web.ViewComponents;


public class HeaderViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View("Header");
    }
}


public class FooterViewComponent : ViewComponent
{
    private readonly ISiteService _siteService;

    public FooterViewComponent(ISiteService siteService)
    {
        _siteService = siteService;
    }
    public IViewComponentResult Invoke()
    {
        return View("Footer", _siteService.GetDefaultSiteInformation());
    }
}

