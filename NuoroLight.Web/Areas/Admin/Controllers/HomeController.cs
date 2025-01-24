using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
namespace NuoroLight.Web.Areas.Admin.Controllers;
public class HomeController : AdminBaseController
{



    #region Index
    public IActionResult Index() => View();
    #endregion

    #region Error404
    [HttpGet("error-404")]
    public IActionResult Error404() => View();
    #endregion


}

