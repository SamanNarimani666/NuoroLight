using NuoroLight.Domain.Entities.Permissions;

namespace NuoroLight.Domain.IRepositories;
public interface IPermissionRepository:IAsyncDisposable
{
    Task<List<Permission>> GetAllPermission();

}
