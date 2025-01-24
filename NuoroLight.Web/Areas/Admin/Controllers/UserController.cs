using Microsoft.AspNetCore.Mvc;
using NuoroLight.Application.Services.Interfaces;
using NuoroLight.Domain.ViewModels.User;
namespace NuoroLight.Web.Areas.Admin.Controllers;
[AutoValidateAntiforgeryToken]
public class UserController : AdminBaseController
{
    #region Filed
    private readonly IUserService _userService;
    private readonly IPermissionService _permissionService;
    #endregion

    #region Constructor
    public UserController(IUserService userService, IPermissionService permissionService)
    {
        _userService = userService;
        _permissionService = permissionService;
    }
    #endregion

    #region UserList
    [HttpGet("User-list")]
    public async Task<IActionResult> UserList(FilterUserViewModel filterUser)
    {
        return View(await _userService.FilterUser(filterUser));
    }
    #endregion

    #region DeleteUser
    [HttpPost("delete-user"),ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(DeleteAndRestoreUserViewModel deleteAndRestoreUser)
    {
        var res = await _userService.DeleteUser(deleteAndRestoreUser.UserId);
        if (res)
        {
            TempData[SuccessMessage] = "کاربر مورد تظر با موفقیت حذف شد";
        }
        else
        {
            TempData[ErrorMessage] = "خطا در حذف کاربر";

        }
        return RedirectToAction("UserList", "User");
    }
    #endregion

    #region RestoreteUser
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> RestoreteUser(DeleteAndRestoreUserViewModel deleteAndRestoreUser)
    {
        var res = await _userService.RestoreUser(deleteAndRestoreUser.UserId);
        if (res)
        {
            TempData[SuccessMessage] = "کاربر مورد تظر با موفقیت بازگردانده شد";
        }
        else
        {
            TempData[ErrorMessage] = "خطا در بازگرداندن کاربر";
        }
        return RedirectToAction("UserList", "User");
    }
    #endregion

    #region Create User
    [HttpGet("create-user")]
    public async Task<IActionResult> CreateUser()
    {
        ViewBag.Permission = await _permissionService.GetAllPermission();
        return View();
    }
    [HttpPost("create-user"), ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateUser(CreateUserViewModel createUser,List<int> SelectedPermission, IFormFile userAvatar)
    {
        if (ModelState.IsValid)
        {
            var res = await _userService.CreateUser(createUser, SelectedPermission, userAvatar);
            switch (res)
            {
                case CreateUserResult.Error:
                    TempData[ErrorMessage] = "خطا در ایجاد کاربر جدید";
                    break;
                case CreateUserResult.ExistMobile:
                    TempData[ErrorMessage] = "کاربری با این شماره تماس موجود می باشد";
                    break;
                case CreateUserResult.ExistUserName:
                    TempData[ErrorMessage] = "نام کاربری وارد شده تکراری می باشد";
                    break;
                case CreateUserResult.ExistsEmail:
                    TempData[ErrorMessage] = "ایمیل وارد شده تکراری می باشد";
                    break;
                case CreateUserResult.Success:
                    TempData[SuccessMessage] = "کاربر جدید با موفقیت ثبت شد";
                    return RedirectToAction("UserList", "User");
            }
        }
        ViewBag.Permission = await _permissionService.GetAllPermission();
        return View(createUser);
    }
    #endregion

    #region EditUser
    [HttpGet("edit-user/{userId}")]
    public async Task<IActionResult> EditUser(int userId)
    {
        ViewBag.Permission = await _permissionService.GetAllPermission();
        ViewBag.UserPermission = _permissionService.UserRolesId(userId);

        return View(await _userService.UserInfoForEdit(userId));
    }
    [HttpPost("edit-user/{userId}"), ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUser(int userId, EditUserForAdminViewModel editUserInfo, List<int> SelectedPermission, IFormFile? userAvatar)
    {
        if (ModelState.IsValid)
        {
            var res = await _userService.EditUserForAdmin(editUserInfo, SelectedPermission, userAvatar);
            switch (res)
            {
                case EditUserResult.Error:
                    TempData[ErrorMessage] = "خطا در ویرایش کاربر";
                    break;
                case EditUserResult.NotFound:
                    TempData[ErrorMessage] = "کاربری با این مشخصات یافت نشد";
                    break;
                case EditUserResult.Success:
                    TempData[SuccessMessage] = "ویرایش کاربر با موفقیت ثبت شد";
                    return RedirectToAction("UserList", "User");
            }
        }
        ViewBag.Permission = await _permissionService.GetAllPermission();
        ViewBag.UserPermission = _permissionService.UserRolesId(userId);
        return View(editUserInfo);
    }
    #endregion

    #region UserDetials
    [HttpGet("user-details/{userId}")]
    public async Task<IActionResult> UserDetials(int userId)
    {
        var user = await _userService.GetUserDetialByUserId(userId: userId);
        if (user == null) return NoContent();
        return View(user);
    }
    #endregion

}

