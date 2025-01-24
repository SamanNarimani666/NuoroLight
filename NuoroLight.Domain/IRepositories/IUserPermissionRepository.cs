namespace NuoroLight.Domain.IRepositories;
public interface IUserPermissionRepository:IAsyncDisposable
{
    Task AddUserPermission(List<int> permissionIds,int userId);
    List<int> UserRolesId(int userId);
    void RemoveUserPermission(int userId);
    Task<bool> CheckUserPermission(int userId, int permissionId);
    Task Save();
}
