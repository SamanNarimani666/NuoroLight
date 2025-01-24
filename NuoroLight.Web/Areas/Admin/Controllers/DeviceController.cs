using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using nuorolight.web;
using NuoroLight.Application.Services.Interfaces;
using NuoroLight.Domain.ViewModels.Device;
using NuoroLight.Domain.ViewModels.Sensor;
using NuoroLight.Web.Hubs;

namespace NuoroLight.Web.Areas.Admin.Controllers;
[AutoValidateAntiforgeryToken]

public class DeviceController : AdminBaseController
{
    #region Field
    private readonly IDeviceService _deviceService;

    #endregion

    #region Constructor
    public DeviceController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }
    #endregion

    #region Index
    [HttpGet]
    [Route("Admin/Device/Index")]
    public async Task<IActionResult> Index(FilterDeviceViewModel filter)
    {
        return View(await _deviceService.FilterDevice(filter));
    }
    #endregion

    #region CreateDevice
    [HttpGet("create-device")]
    public IActionResult CreateDevice()
    {
        return View();
    }

    [HttpPost("create-device")]
    public async Task<IActionResult> CreateDevice(CreateDeviceViewModel createDevice)
    {
        if (ModelState.IsValid)
        {
            switch (await _deviceService.CreateDevice(createDevice))
            {
                case CreateDeviceResult.ExistsDeviceName:
                    TempData[WarningMessage] = "نام دستگاه تکراری می باشد";
                    break;
                case CreateDeviceResult.ExistsDeviceMAC:
                    TempData[WarningMessage] = "MAC وارد شده متعلق به دستگاه دیگری می باشد";
                    break;
                case CreateDeviceResult.Error:
                    TempData[ErrorMessage] = "خطا در انجام عملیات";
                    break;
                case CreateDeviceResult.Success:
                    TempData[SuccessMessage] = $"دستگاه {createDevice.Name} با موفقیت ثبت شد";
                    return RedirectToAction("Index");
            }
        }
        return View(createDevice);
    }
    #endregion

    #region GetDeviceSensor
    [HttpGet("get-sensor/{deviceId}/{deviceName}")]
    public async Task<IActionResult> GetDeviceSensor(int deviceId, string deviceName)
    {
        ViewData["DeviceName"] = deviceName;
        ViewData["DeviceId"] = deviceId;
        return View(await _deviceService.GetDeviceSesnsors(deviceId));
    }
    #endregion

    #region CreateSensor
    [HttpGet("create-sensor/{deviceId}")]
    public IActionResult CreateSensor(int deviceId)
    {
        return View(new CreateSensorViewModel { DeviceId = deviceId });
    }


    [HttpPost("create-sensor/{deviceId}")]
    public async Task<IActionResult> CreateSensor(int deviceId, CreateSensorViewModel createSensor)
    {
        if (ModelState.IsValid)
        {
            switch (await _deviceService.CreateSensor(createSensor))
            {
                case CreateSensorResult.EmptyCommands:
                    TempData[WarningMessage] = $"دستورات مربوط به سنسور {createSensor.Name} را وارد نمایید";
                    break;

                case CreateSensorResult.Error:
                    TempData[WarningMessage] = "خطا در انجام عملیات";
                    break;
                case CreateSensorResult.Success:
                    TempData[SuccessMessage] = "سنسور با موفقیت ثبت شد";
                    return RedirectToAction("GetDeviceSensor", new { deviceId = deviceId, deviceName = createSensor.Name });
            }
        }
        return View();
    }
    #endregion


    #region FilterLogs
    [Route("Admin/Device/logs")]
    public async  Task<IActionResult> FilterLogs(FilterDeviceLogsViewModel filter)
    {
        return View(await _deviceService.FilterDeviceLogs(filter));
    }
    #endregion
}
