using System.ComponentModel.DataAnnotations;
using NuoroLight.Domain.Entities.User;
namespace NuoroLight.Domain.ViewModels.Account;
public class EditUserProfileViewModel
{
    [Display(Name = "ایمیل")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد.")]
    public string Email { get; set; }

    [Display(Name = "نام ")]
    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public string FirstName { get; set; }
    [Display(Name = "نام خانوادگی")]
    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public string LastName { get; set; }

    public string? AvatarName { get; set; }

    [Display(Name = "جنسیت")]
    public Gender Gender { get; set; }

    [Display(Name = "آدرس سایت")]
    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
    public string? SiteUrl { get; set; }
}
public enum EditUserProfileResult
{
    Success,
    NotFound,
    NotActive,
    UserIsBlock,
    NotIsIamage,
    EmailIsExist,
    Error
}