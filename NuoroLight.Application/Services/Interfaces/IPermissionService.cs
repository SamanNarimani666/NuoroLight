using NuoroLight.Domain.Entities.Permissions;

namespace NuoroLight.Application.Services.Interfaces;
public interface IPermissionService:IAsyncDisposable
{
    Task<List<Permission>> GetAllPermission();
    public List<int> UserRolesId(int userId);
    Task<bool> CheckUserPermission(int userId, int permissionId);
}

