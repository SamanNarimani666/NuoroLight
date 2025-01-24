using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuoroLight.Domain.Entities.Permissions;
[Table(name: "Permission", Schema = "Users")]
public class Permission
{
    [Key]
    public int PermissionId { get; set; }

    [Display(Name = "نام دسترسی")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
    public string PermissionName { get; set; }

    public int? ParentID { get; set; }


    [ForeignKey("ParentID")]
    public List<Permission> Permissions { get; set; }


    public ICollection<UserPermission> UserPermissions { get; set; }

}

