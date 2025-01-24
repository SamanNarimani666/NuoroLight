using System.ComponentModel.DataAnnotations;

namespace NuoroLight.Domain.ViewModels.Device;
public class CreateDeviceViewModel
{
    [Display(Name = "نام دستگاه")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(100)]
    public string Name { get; set; } // نام دستگاه

    [Display(Name = "MAC")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(17)] // فرمت استاندارد MAC Address (XX:XX:XX:XX:XX:XX)
    public string MAC { get; set; } // آدرس MAC دستگاه


    [Display(Name = "مکان")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(100)]
    public string Location { get; set; } // مکان دستگاه

    [Display(Name = "وضعیت")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public bool IsActive { get; set; }
}

public enum CreateDeviceResult
{
    Success,
    ExistsDeviceName,
    ExistsDeviceMAC,
    Error
}

