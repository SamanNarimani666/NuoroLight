using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace NuoroLight.Domain.Entities.SiteSetting;
[Table(name: "SiteSetting", Schema = "Site")]
public class SiteSetting
{
    [Key]
    public int SiteSettingId { get; set; }
    [Display(Name = "تلفن همراه")]
    public string Mobile { get; set; }

    [Display(Name = "تلفن")]
    public string Phone { get; set; }

    [Display(Name = "آدرس ایمیل")]
    public string Email { get; set; }

    [Display(Name = "متن فوتر")]
    public string FooterText { get; set; }

    [Display(Name = "متن کپی رایت")]
    public string CopyRight { get; set; }

    [Display(Name = "درباره ما")]
    public string AboutUs { get; set; }

    [Display(Name = "آدرس")]
    public string Address { get; set; }

    [Display(Name = "اصلی هست / نیست")]
    public bool IsDefault { get; set; }

}
