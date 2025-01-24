using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NuoroLight.Domain.Entities.Device;
using NuoroLight.Domain.Entities.Permissions;

namespace NuoroLight.Domain.Entities.User;
[Table(name:"User",Schema = "Users")]
public class User
{
    [Key]
    public int UserId { get; set; }
    [Display(Name = "نام کاربری")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
    public string UserName { get; set; }
    [Display(Name = "ایمیل")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
    [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Display(Name = "کلمه ی عبور")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
    public string Password { get; set; }

    [Display(Name = "نام")]
    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
    public string? FirstName { get; set; }

    [Display(Name = "نام خانوادگی")]
    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
    public string? LastName { get; set; }

    [Display(Name = "نام کامل")]
    public string? FullName => $"{FirstName} {LastName}".Trim();

    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
    public string ActiveCode { get; set; }

    [Display(Name = "تصویر آواتار")]
    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
    public string? Avatar { get; set; }

    [Display(Name = "تلفن همراه")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
    [MinLength(11, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد")]
    public string Mobile { get; set; }

    [Display(Name = "جنسیت")]
    public Gender? Gender { get; set; }

    [Display(Name = "آدرس سایت")]
    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
    public string? SiteUrl { get; set; }

    [Display(Name = " فعال / غیرفعال")]
    public bool IsActive { get; set; }
    [Display(Name = "بلاک شده / نشده")]
    public bool IsBlock { get; set; }
    [Display(Name = "حذف شده / نشده")]
    public bool IsDelete { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}

public enum Gender
{
    [Display(Name = "نامشخص")]
    Unknown,
    [Display(Name = "مرد")]
    Male,
    [Display(Name = "زن")]
    Female
}

