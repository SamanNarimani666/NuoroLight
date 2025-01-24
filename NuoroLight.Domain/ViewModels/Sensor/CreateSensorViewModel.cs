
using NuoroLight.Domain.Entities.Device;
using NuoroLight.Domain.ViewModels.Command;
using System.ComponentModel.DataAnnotations;

namespace NuoroLight.Domain.ViewModels.Sensor;
public class CreateSensorViewModel
{

    public int DeviceId { get; set; }

    [MaxLength(100)]
    [Display(Name = "نام سنسور")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public string Name { get; set; } // نام سنسور (مثلاً دما، رطوبت)

    [Display(Name = "نوع سنسور")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public SensorType SensorType { get; set; } //enum

    public List<CreateCommandViewModel> Commands { get; set; }
}


public enum CreateSensorResult
{
    Success,
    EmptyCommands,
    Error
}