using System.ComponentModel.DataAnnotations;

namespace NuoroLight.Domain.ViewModels.Command;
public class CreateCommandViewModel
{
    [Display(Name = "عملیات دستور")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public string Display { get; set; }

    [Display(Name = "دستور")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    // متن واقعی دستور که به سنسور ارسال می‌شود
    public string Value { get; set; }



    [Display(Name = "توضیحات")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    // متن واقعی دستور که به سنسور ارسال می‌شود
    public string Description { get; set; }

}
