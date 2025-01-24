using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace NuoroLight.Web.Areas.Admin.Controllers;
[Authorize]
[Area("Admin")]
[Route("Admin")]
public class AdminBaseController : Controller
{
    protected string ErrorMessage = "ErrorMessage";
    protected string SuccessMessage = "SuccessMessage";
    protected string WarningMessage = "WarningMessage";
    protected string InfoMessage = "InfoMessage";
}

