using System.ComponentModel.DataAnnotations;
using NuoroLight.Domain.ViewModels.Site;
namespace NuoroLight.Domain.ViewModels.Account;
public class LoginViewModel 
{
    [Display(Name = "ایمیل")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد.")]
    public string Email { get; set; }

    [Display(Name = "کلمه عبور")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public string PassWord { get; set; }

    [Display(Name = "مرا به خاطر بسپار")]
    public bool RememberMe { get; set; }
}
public enum LoginResult
{
    Success,
    NotFound,
    NotActive,
    DeletedAccount,
    UserIsBlock,
}
