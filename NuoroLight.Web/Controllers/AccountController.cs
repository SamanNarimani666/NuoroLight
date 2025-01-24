﻿using System.Security.Claims;
using NuoroLight.Web.HttpContext;
using GoogleReCaptcha.V3.Interface;
using Hangfire;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NuoroLight.Domain.ViewModels.Account;
using NuoroLight.Application.Senders;
using NuoroLight.Core.Convertors;
using NuoroLight.Application.Services.Interfaces;
using NuoroLight.Application.ViewModels.Account;
namespace NuoroLight.Web.Controllers;
[AutoValidateAntiforgeryToken]
public class AccountController : SiteBaseController
{
    #region Field
    private readonly IUserService _userService;
    private readonly ISender _sender;
    private readonly IViewRenderService _viewRenderService;
    private readonly ICaptchaValidator _captchaValidator;
    private readonly IBackgroundJobClient _backgroundJobClient;
    #endregion

    #region Constructor
    public AccountController(IUserService userService, ISender sender,
        IViewRenderService viewRenderService, ICaptchaValidator captchaValidator,
        IBackgroundJobClient backgroundJobClient)
    {
        _userService = userService;
        _sender = sender;
        _viewRenderService = viewRenderService;
        _captchaValidator = captchaValidator;
        _backgroundJobClient = backgroundJobClient;
    }
    #endregion

    #region Register
    [RedirectIfLoggedInActionFilter]
    [HttpGet("Register")]
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost("Register"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel registerUser)
    {
        if (ModelState.IsValid)
        {
            var res = await _userService.RegisterUser(registerUser);
            switch (res)
            {
                case RegisterResult.ExistMobile:
                    TempData[WarningMessage] = "شماره موبایل وارد شده قبلا استفاده شده است";
                    break;
                case RegisterResult.ExistUserName:
                    TempData[WarningMessage] = "نام کاربری متعلق به کاربر دیگری می باشد";
                    break;
                case RegisterResult.ExistsEmail:
                    TempData[WarningMessage] = "ایمیل وارد شده قبلا استفاده شده است";
                    break;
                case RegisterResult.Failed:
                    TempData[ErrorMessage] = "خطا در ثبت نام";
                    break;
                case RegisterResult.Success:
                    TempData[SuccessMessage] = "ثبت نام با موفقیت انجام شد جهت فعال سازی حساب کاربری خود ایمیل های خود را برسی کنید";
                    var user = await _userService.GetUserByEmail(registerUser.Email);
                    var body = await _viewRenderService.RenderToStringAsync("Emails/ActivateEmail", user);
                    _backgroundJobClient.Enqueue(() => _sender.SendEmail(registerUser.Email, "فعال سازی حساب کاربری", body));
                    return RedirectToAction("Index", "Home");

            }
        }
        return View(registerUser);
    }

    #endregion

    #region ActiveAccount
    [HttpGet("activate-account/{activecode}")]
    public async Task<IActionResult> ActiveAccount(string activecode)
    {
        ViewBag.IsActive = await _userService.ActiveUserByActiveCode(activecode);
        return View();
    }
    #endregion

    #region Login
    [RedirectIfLoggedInActionFilter]
    [HttpGet("Login")]
    public IActionResult Login(string? ReturnUrl)
    {
        ViewBag.returnUrl = ReturnUrl;
        return View();
    }
    [HttpPost("Login"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Login([Bind("Email,PassWord,RememberMe")] LoginViewModel login, string? ReturnUrl)
    {
        if (ModelState.IsValid)
        {
            var res = await _userService.LoginUser(login);
            switch (res)
            {
                case LoginResult.NotActive:
                    TempData[WarningMessage] = "حساب کاربری شما فعال نشده است";
                    break;
                case LoginResult.NotFound:
                    TempData[WarningMessage] = "حساب کاربری با این مشخصات یافت نشد";
                    break;
                case LoginResult.UserIsBlock:
                    TempData[WarningMessage] = "حساب کاربری شما بلاک شده است";
                    break;
                case LoginResult.DeletedAccount:
                    TempData[WarningMessage] = "حساب کاربری شما حذف شده است";
                    break;
                case LoginResult.Success:
                    TempData[SuccessMessage] = "ورود به لایت نورو موفقیت انجام شد";
                    var user = await _userService.GetUserByEmail(login.Email);
                    var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim(ClaimTypes.Name,user.UserName),
                            new Claim(ClaimTypes.MobilePhone,user.Mobile)
                        };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    var properties = new AuthenticationProperties
                    {
                        IsPersistent = login.RememberMe
                    };
                    await HttpContext.SignInAsync(principal, properties);
                    if (!string.IsNullOrEmpty(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    return RedirectToAction("Index", "Home");
            }
        }
        return View(login);
    }
    #endregion

    #region ForgotPass
    [HttpGet("ForgotPass")]
    public IActionResult ForgotPass()
    {
        return View();
    }
    [HttpPost("ForgotPass")]
    public async Task<IActionResult> ForgotPass(ForgotPassViewModel forgotPass)
    {
        if (!await _captchaValidator.IsCaptchaPassedAsync(forgotPass.Captcha))
        {
            TempData[ErrorMessage] = "کد کپچای شما تایید نشد";
        }
        if (ModelState.IsValid)
        {
            var res = await _userService.ForgotPassWordUser(forgotPass);
            switch (res)
            {
                case ForgotPassResult.UserIsBlock:
                    TempData[WarningMessage] = "حساب کاربری شما بلاک شده است";
                    break;
                case ForgotPassResult.NotActive:
                    TempData[WarningMessage] = "حساب کاربری شما فعال نشده است";
                    break;
                case ForgotPassResult.NotFount:
                    TempData[ErrorMessage] = "حساب کاربری با این ایمیل یافت نشد";
                    break;
                case ForgotPassResult.FindUser:
                    TempData[SuccessMessage] = "ایمیلی جهت بازیابی کلمه عبور به شما ارسال شد ";
                    var user = await _userService.GetUserByEmail(forgotPass.Email);
                    var body = await _viewRenderService.RenderToStringAsync("Emails/ForgotPassword", user);
                    _backgroundJobClient.Enqueue(() => _sender.SendEmail(forgotPass.Email, "بازیابی کلمه عبور", body));
                    //_backgroundJobClient.Schedule(() => _userService.EditActiveCodeByEmail(forgotPass.Email), TimeSpan.FromMinutes(1));
                    return RedirectToAction("Index", "Home");
            }
        }
        return View(forgotPass);
    }
    #endregion

    #region ResetPassword 
    [HttpGet("reset-pass/{activeCode}")]
    public async Task<IActionResult> ResetPassword(string activeCode)
    {
        if ((string.IsNullOrEmpty(activeCode)) &&
            !await _userService.IsExistsUserByActiveCode(activeCode)) return NotFound();
        return View(new ResetPsssWordViewModel() { ActiveCode = activeCode });
    }
    [HttpPost("reset-pass/{activeCode}"), ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPsssWordViewModel resetPsssWord)
    {
        if (!await _captchaValidator.IsCaptchaPassedAsync(resetPsssWord.Captcha))
        {
            TempData[ErrorMessage] = "کد کپچای شما تایید نشد";
        }

        if (ModelState.IsValid)
        {
            var res = await _userService.ResetPsssWordUser(resetPsssWord);
            switch (res)
            {
                case ResetPsssWordResult.NotFound:
                    TempData[WarningMessage] = "خطا در بازیابی کلمه عبور";
                    break;
                case ResetPsssWordResult.Success:
                    TempData[SuccessMessage] = "بازبابی کلمه عبور با موفقیت انجام شد";
                    return RedirectToAction("Login", "Account");
            }
        }
        return View(resetPsssWord);
    }
    #endregion

    #region LogOut
    [HttpGet("Logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/");
    }
    #endregion
}
