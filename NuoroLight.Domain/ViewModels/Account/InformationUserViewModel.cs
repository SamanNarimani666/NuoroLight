using System.ComponentModel.DataAnnotations;
namespace NuoroLight.Domain.ViewModels.Account;
public class InformationUserViewModel
{
    [Display(Name = "ایمیل")]
    public string Email { get; set; }
    [Display(Name = "شماره تماس")]
    public string Mobile { get; set; }
    [Display(Name = "نام و نام خانوادگی")]
    public string? FullName { get; set; }

    [Display(Name ="آواتار")]
    public string Avatar { get; set; }

    [Display(Name = "نام کاربری")]
    public string UserName { get; set; }
    [Display(Name = "تاریخ عضویت")]
    public DateTime CreatedDate { get; set; }
    public int WalletAmount { get; set; }
}

