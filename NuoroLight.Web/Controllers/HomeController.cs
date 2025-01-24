using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nuorolight.web;
using NuoroLight.Application.Services.Interfaces;

namespace NuoroLight.Web.Controllers;
public class HomeController : SiteBaseController
{
    #region Field
    private readonly ISiteService _siteService;
    private readonly MqttService _mqttService;

    #endregion

    #region Constructor
    public HomeController(ISiteService siteService, MqttService mqttService)
    {
        _siteService = siteService;
        _mqttService = mqttService;
    }
    #endregion

    #region Index
    public async Task<IActionResult> Index()
    {
        return View();
    }
    #endregion

    #region Error404
    public IActionResult Error404() => View();
    #endregion

    #region ContactUs
    //[HttpGet("contact-us")]
    //public async Task<IActionResult> ContactUs()
    //{
    //    ViewBag.SiteInfo = await _siteService.GetDefaultSiteSetting();
    //    return View();
    //}
    //[HttpPost("contact-us"), ValidateAntiForgeryToken]
    //public async Task<IActionResult> ContactUs(CreateContactUsViewModel contactUs)
    //{
    //    if (!await _captchaValidator.IsCaptchaPassedAsync(contactUs.Captcha))
    //    {
    //        TempData[ErrorMessage] = "کد کپچای شما تایید نشد";
    //    }
    //    if (ModelState.IsValid)
    //    {
    //        var res = await _siteService.CreateContact(contactUs, HttpContext.GetUserIp());
    //        switch (res)
    //        {
    //            case CreateContactResult.Error:
    //                TempData[ErrorMessage] = "خطا در ثبت اطلاعات";
    //                break;
    //            case CreateContactResult.Success:
    //                TempData[SuccessMessage] = "تماس شما با موفیت ثبت شد";
    //                return RedirectToAction("Index", "Home");
    //        }
    //    }
    //    ViewBag.SiteInfo = await _siteService.GetDefaultSiteSetting();
    //    return View(contactUs);
    //}
    #endregion

    #region AboutUs
    [HttpGet("about-us")]
    public async Task<IActionResult> AboutUs()
    {
        return View( _siteService.GetDefaultSiteSetting());
    }
    #endregion

    #region SendCommand
    [AllowAnonymous]
    [HttpPost("send-command")]
    public async Task<IActionResult> SendCommand([FromBody] MqttMessageRequest request)
    {
        // بررسی اعتبارسنجی ورودی
        if (request == null || string.IsNullOrWhiteSpace(request.Topic) || string.IsNullOrWhiteSpace(request.Message))
        {
            return BadRequest(new { Error = "دیتا های ارسالی اشتباه می باشد" });
        }

        try
        {
            // اتصال به سرور MQTT
            await _mqttService.ConnectAsync();

            // ارسال پیام
            await _mqttService.PublishAsync(request.Topic, request.Message);


            // بازگشت پاسخ موفقیت
            return Ok(new { Message = "دستور ارسال شد", Topic = request.Topic });
        }
        catch (Exception ex)
        {
            // مدیریت خطا و بازگشت پاسخ مناسب
            // می‌توانید از لاگ برای ثبت خطا استفاده کنید
            return StatusCode(500, new { Error = "خطای سیستمی ", Details = ex.Message });
        }
    }

    #endregion

}

