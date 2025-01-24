
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuoroLight.Domain.Entities.Device;

[Table(name: "Command", Schema = "Devices")]

public class Command
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CommandId { get; set; }
    // نامی که برای نمایش دستور در UI استفاده می‌شود
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

    // شناسه سنسور که دستور به آن ارسال می‌شود 
    public int SensorId { get; set; }

    // ارتباط با سنسور 
    [ForeignKey("SensorId")]

    public virtual Sensor Sensor { get; set; }

}

