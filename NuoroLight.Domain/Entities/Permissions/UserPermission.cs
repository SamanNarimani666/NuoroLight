using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuoroLight.Domain.Entities.Permissions;
[Table(name: "UserPermission", Schema = "Users")]
public class UserPermission
{
    [Key]
    public int UserPermissionId { get; set; }

    public int UserId { get; set; }
    public User.User User { get; set; }
    public int PermissionId { get; set; }
    public Permission Permission { get; set; }

}

