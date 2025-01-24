using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NuoroLight.Domain.Entities.Device;
[Table(name: "Sensor", Schema = "Devices")]

public class Sensor
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SensorId { get; set; } // کلید اصلی جدول سنسورها

    [MaxLength(100)]
    [Display(Name = "نام سنسور")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public string Name { get; set; } // نام سنسور (مثلاً دما، رطوبت)

    [Display(Name = "نوع سنسور")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public SensorType SensorType { get; set; }

    public bool IsDelete { get; set; }


    [Required]
    public int DeviceId { get; set; } // کلید خارجی مرتبط با دستگاه

    [ForeignKey("DeviceId")]
    public virtual Device Device { get; set; } // ارتباط با جدول دستگاه‌ها


    public ICollection<Command> Commands { get; set; }
}


public enum SensorType
{
    [Display(Name = "رله")]
    Relay,

    [Display(Name = "RGB")]
    RGB,

    [Display(Name = "تشخیص حرکت PIR")]
    PIR,

    [Display(Name = "سنسور دما")]
    Temperature,

    [Display(Name = "سنسور رطوبت")]
    Humidity,

    [Display(Name = "سنسور فشار")]
    Pressure,

    [Display(Name = "سنسور نور")]
    Light,

    [Display(Name = "سنسور حرکت")]
    Motion,

    [Display(Name = "سنسور گاز")]
    Gas,

    [Display(Name = "سنسور مجاورت")]
    Proximity,

    [Display(Name = "سنسور صدا")]
    Sound,

    [Display(Name = "سنسور لرزش")]
    Vibration,

    [Display(Name = "سنسور دود")]
    Smoke,

    [Display(Name = "سنسور سطح آب")]
    WaterLevel,

    [Display(Name = "سنسور دی‌اکسید کربن")]
    CO2,

    [Display(Name = "سنسور شعله")]
    Flame,

    [Display(Name = "سنسور ضربان قلب")]
    HeartRate,

    [Display(Name = "سنسور موقعیت مکانی")]
    GPS,

    [Display(Name = "سنسور رطوبت خاک")]
    SoilMoisture,

    [Display(Name = "سنسور شتاب‌سنج")]
    Accelerometer,

    [Display(Name = "سنسور ژیروسکوپ")]
    Gyroscope,

    [Display(Name = "سنسور میدان مغناطیسی")]
    MagneticField,

    [Display(Name = "سنسور اولتراسونیک")]
    Ultrasonic,

    [Display(Name = "سنسور RFID")]
    RFID,

    [Display(Name = "سنسور اثر انگشت")]
    Fingerprint
}
