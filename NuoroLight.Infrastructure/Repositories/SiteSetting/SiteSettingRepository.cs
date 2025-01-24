using Microsoft.EntityFrameworkCore;
using NuoroLight.Domain.IRepositories;
using NuoroLight.Infrastructure.Context;

namespace NuoroLight.Data.Repositories.SiteSetting;
public class SiteSettingRepository : ISiteSettingRepository
{
    #region Field
    private readonly NuoroLightContext _context;
    #endregion

    #region Constructor
    public SiteSettingRepository(NuoroLightContext context)
    {
        this._context = context;
    }
    #endregion

    #region GetDefaultSiteSetting
    public Domain.Entities.SiteSetting.SiteSetting GetDefaultSiteSetting()
    {
        return _context.SiteSettings.FirstOrDefault(s => s.IsDefault);
    }
    #endregion

    #region EditSiteSetting
    public void EditSiteSetting(Domain.Entities.SiteSetting.SiteSetting siteSetting)
    {
        _context.SiteSettings.Update(siteSetting);
    }
    #endregion

    #region GetDefaultSiteSetting
    public async Task<Domain.Entities.SiteSetting.SiteSetting> GetDefaultSiteSettingById(int siteSettingId)
    {
        return await _context.SiteSettings.FirstOrDefaultAsync(s => s.SiteSettingId == siteSettingId && s.IsDefault);
    }
    #endregion

    #region Save
    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }
    #endregion

    #region Dispose
    public async ValueTask DisposeAsync()
    {
        if (_context != null)
        {
            await _context.DisposeAsync();
        }
    }
    #endregion
}

