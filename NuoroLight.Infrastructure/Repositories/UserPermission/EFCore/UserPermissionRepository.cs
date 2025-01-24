using Microsoft.EntityFrameworkCore;
using NuoroLight.Domain.IRepositories;
using NuoroLight.Infrastructure.Context;

namespace NuoroLight.Data.Repositories.UserPermission;
public class UserPermissionRepository : IUserPermissionRepository
{
    #region Constructor
    private readonly NuoroLightContext _context;
    public UserPermissionRepository(NuoroLightContext context)
    {
        _context = context;
    }
    #endregion

    #region DisposeAsync
    public async ValueTask DisposeAsync()
    {
        if (_context != null)
        {
            await _context.DisposeAsync();
        }
    }
    #endregion

    #region AddUserPermission
    public async Task AddUserPermission(List<int> permissionIds, int userId)
    {
        foreach (var permissionId in permissionIds)
        {
            await _context.AddAsync(new Domain.Entities.Permissions.UserPermission()
            {
                UserId = userId,
                PermissionId = permissionId
            });
        }
    }
    #endregion

    #region UserRolesId
    public List<int> UserRolesId(int userId)
    {
        return _context.UserPermissions.Where(p => p.UserId == userId).Select(p => p.PermissionId).ToList();
    }
    #endregion

    #region RemoveUserPermission
    public void RemoveUserPermission(int userId)
    {
        _context.UserPermissions.Where(r => r.UserId == userId).ToList().ForEach(r => _context.UserPermissions.Remove(r));
    }
    #endregion

    #region CheckUserPermission
    public async Task<bool> CheckUserPermission(int userId, int permissionId)
    {
        return await _context.UserPermissions.AnyAsync(p => p.UserId == userId && p.PermissionId == permissionId);
    }
    #endregion

    #region Save
    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }
    #endregion
}


