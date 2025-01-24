using NuoroLight.Application.Convertors;
using NuoroLight.Application.Security;
using NuoroLight.Application.Senders;
using NuoroLight.Application.Services.Interfaces;
using NuoroLight.Domain.Entities.SiteSetting;
using NuoroLight.Domain.IRepositories;
using NuoroLight.Domain.ViewModels.SiteSetting;

namespace NuoroLight.Application.Services.Implementations
{
    public class SiteService : ISiteService
    {
        #region Contrcutor
        private readonly ISiteSettingRepository _settingRepository;
        private readonly ISender _sender;
        public SiteService( ISiteSettingRepository settingRepository, ISender sender)
        {
            _settingRepository = settingRepository;
            _sender = sender;
        }
        #endregion

        #region GetDefaultSiteSetting
        public SiteSetting GetDefaultSiteSetting()
        {
            return  _settingRepository.GetDefaultSiteSetting();
        }
        #endregion

        #region GetDefaultSiteInformation
        public GetSiteInformation GetDefaultSiteInformation()
        {
            var siteSetting =  _settingRepository.GetDefaultSiteSetting();
            if (siteSetting == null) return null;

            return new GetSiteInformation()
            {
                Email = siteSetting.Email,
                Phone = siteSetting.Phone,
                Mobile = siteSetting.Mobile,
                Address = siteSetting.Address,
                AboutUs = siteSetting.AboutUs,
                CopyRight = siteSetting.CopyRight,
                FooterText = siteSetting.FooterText,
                SiteSettingId = siteSetting.SiteSettingId
            };
        }
        #endregion

        #region EditSiteSetting
        public async Task<EditSiteSettingResult> EditSiteSetting(GetSiteInformation editSiteInformation)
        {
            var mainSiteSetting = await _settingRepository.GetDefaultSiteSettingById(editSiteInformation.SiteSettingId);
            if (mainSiteSetting == null) return EditSiteSettingResult.NotFound;
            mainSiteSetting.Phone = editSiteInformation.Phone.SanitizeText();
            mainSiteSetting.Email = FixedText.FixEmail(editSiteInformation.Email.SanitizeText());
            mainSiteSetting.AboutUs = editSiteInformation.AboutUs.SanitizeText();
            mainSiteSetting.CopyRight = editSiteInformation.CopyRight.SanitizeText();
            mainSiteSetting.Address = editSiteInformation.Address.SanitizeText();
            mainSiteSetting.FooterText = editSiteInformation.FooterText.SanitizeText();
            mainSiteSetting.Address = editSiteInformation.Address;
            try
            {
                _settingRepository.EditSiteSetting(mainSiteSetting);
                await _settingRepository.Save();
                return EditSiteSettingResult.Success;
            }
            catch
            {
                return EditSiteSettingResult.Error;
            }
        }
        #endregion

        #region Dispose
        public async ValueTask DisposeAsync()
        {
            await _settingRepository.DisposeAsync();
        }
        #endregion

    }
}
