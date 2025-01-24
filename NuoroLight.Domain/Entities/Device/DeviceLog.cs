using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NuoroLight.Domain.Entities.Device;
[Table(name: "DeviceLog", Schema = "Devices")]
public class DeviceLog
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int LogId { get; set; } // کلید اصلی جدول
    [Display(Name = " متن")]
    [Required]
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; } // تاریخ ایجاد دستگاه
}

