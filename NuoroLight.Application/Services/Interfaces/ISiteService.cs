using NuoroLight.Domain.Entities.SiteSetting;
using NuoroLight.Domain.ViewModels.SiteSetting;

namespace NuoroLight.Application.Services.Interfaces;
public interface ISiteService : IAsyncDisposable
{
    SiteSetting GetDefaultSiteSetting();
    GetSiteInformation GetDefaultSiteInformation();
    Task<EditSiteSettingResult> EditSiteSetting(GetSiteInformation editSiteInformation);
}

