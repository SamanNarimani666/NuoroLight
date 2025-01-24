namespace NuoroLight.Domain.IRepositories;
public interface ISiteSettingRepository : IAsyncDisposable
{
    Domain.Entities.SiteSetting.SiteSetting GetDefaultSiteSetting();
    void EditSiteSetting(Entities.SiteSetting.SiteSetting siteSetting);
    Task<Entities.SiteSetting.SiteSetting> GetDefaultSiteSettingById(int siteSettingId);
    Task Save();
}

