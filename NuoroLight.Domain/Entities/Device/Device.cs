using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuoroLight.Domain.Entities.Device;

[Table(name: "Device", Schema = "Devices")]
public class Device
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DeviceId { get; set; } // کلید اصلی جدول

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } // نام دستگاه

    [Required]
    [MaxLength(17)] // فرمت استاندارد MAC Address (XX:XX:XX:XX:XX:XX)
    public string MAC { get; set; } // آدرس MAC دستگاه

    [Required]
    [MaxLength(100)]
    public string Location { get; set; } // مکان دستگاه

    [Required]
    public bool IsActive { get; set; } // وضعیت فعال یا غیرفعال دستگاه

    [Required]
    public DateTime CreatedAt { get; set; } // تاریخ ایجاد دستگاه

    [Required]
    public DateTime UpdatedAt { get; set; } // تاریخ آخرین به‌روزرسانی

    public bool IsDelete { get; set; }

    

    public virtual ICollection<Sensor> Sensors { get; set; } // رابطه یک به چند با سنسورها
}
