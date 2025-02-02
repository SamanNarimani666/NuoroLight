﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NuoroLight.Web.Controllers;
public class SiteBaseController : Controller
{
    protected string ErrorMessage = "ErrorMessage";
    protected string SuccessMessage = "SuccessMessage";
    protected string WarningMessage = "WarningMessage";
    protected string InfoMessage = "InfoMessage";
    protected string BasketCookieKey = "cart-items";
}
