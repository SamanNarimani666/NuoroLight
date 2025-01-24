using NuoroLight.Domain.Entities.Device;
using NuoroLight.Domain.ViewModels.Paging;
using System.ComponentModel.DataAnnotations;

namespace NuoroLight.Domain.ViewModels.Device
{
    public class FilterDeviceLogsViewModel : BasePaging
    {
        public FilterDeviceLogOrderBy OrderBy { get; set; }

        public List<DeviceLog> DeviceLogs { get; set; }

        #region Methods

        public FilterDeviceLogsViewModel SetUser(List<NuoroLight.Domain.Entities.Device.DeviceLog> deviceLogs)
        {
            this.DeviceLogs = deviceLogs;
            return this;
        }
        public FilterDeviceLogsViewModel SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.AllEntitiesCount = paging.AllEntitiesCount;
            this.StartPage = paging.StartPage;
            this.EndPage = paging.EndPage;
            this.PageCount = paging.PageCount;
            this.HowManyShowPageAfterAndBefore = paging.HowManyShowPageAfterAndBefore;
            this.TakeEntity = paging.TakeEntity;
            this.SkipEntity = paging.SkipEntity;
            return this;
        }

        #endregion
    }

    public enum FilterDeviceLogOrderBy
    {
        [Display(Name = "نزولی ")]
        Create_Date_Desc,
        [Display(Name = " صعودی")]
        Create_Date_Asc,
    }
}
