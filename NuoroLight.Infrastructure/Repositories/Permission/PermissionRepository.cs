using Microsoft.EntityFrameworkCore;
using NuoroLight.Domain.IRepositories;
using NuoroLight.Infrastructure.Context;
namespace NuoroLight.Data.Repositories.Permission;
public class PermissionRepository : IPermissionRepository
{
    #region Constructor
    private readonly NuoroLightContext _context;
    public PermissionRepository(NuoroLightContext context)
    {
        _context = context;
    }
    #endregion

    #region GetAllPermission
    public async Task<List<Domain.Entities.Permissions.Permission>> GetAllPermission()
    {
        return await _context.Permissions.ToListAsync();
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
}

