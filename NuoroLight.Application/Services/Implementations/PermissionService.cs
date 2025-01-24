using NuoroLight.Application.Services.Interfaces;
using NuoroLight.Domain.Entities.Permissions;
using NuoroLight.Domain.IRepositories;

namespace NuoroLight.Application.Services.Implementations;
public class PermissionService:IPermissionService
{
    #region Filed
    private readonly IPermissionRepository _permissionRepository;
    private readonly IUserPermissionRepository _userPermissionRepository;
    #endregion

    #region Constructor
    public PermissionService(IPermissionRepository permissionRepository, IUserPermissionRepository userPermissionRepository)
    {
        _permissionRepository = permissionRepository;
        _userPermissionRepository = userPermissionRepository;

    }
    #endregion

    #region GetAllPermission
    public async Task<List<Permission>> GetAllPermission()
    {
        return await _permissionRepository.GetAllPermission();
    }
    #endregion

    #region UserRolesId
    public List<int> UserRolesId(int userId)
    {
        return _userPermissionRepository.UserRolesId(userId);
    }
    #endregion

    #region CheckUserPermission
    public async Task<bool> CheckUserPermission(int userId, int permissionId)
    {
        return await _userPermissionRepository.CheckUserPermission(userId, permissionId);
    }
    #endregion

    #region Dispose
    public async ValueTask DisposeAsync()
    {
        await _permissionRepository.DisposeAsync();
        await _userPermissionRepository.DisposeAsync();
    }
    #endregion
}

