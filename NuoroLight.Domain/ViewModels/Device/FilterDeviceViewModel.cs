using NuoroLight.Domain.ViewModels.Paging;
using NuoroLight.Domain.ViewModels.User;
using System.ComponentModel.DataAnnotations;

namespace NuoroLight.Domain.ViewModels.Device
{
    public class FilterDeviceViewModel : BasePaging
    {
        public string? Name { get; set; }

        public FilterDeviceOrderBy DeviceOrderBy { get; set; }


        public List<Domain.Entities.Device.Device> Devices { get; set; }

        #region Methods

        public FilterDeviceViewModel SetUser(List<NuoroLight.Domain.Entities.Device.Device> devices)
        {
            this.Devices = devices;
            return this;
        }
        public FilterDeviceViewModel SetPaging(BasePaging paging)
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


    public enum FilterDeviceOrderBy
    {
        [Display(Name = "نزولی ")]
        Create_Date_Desc,
        [Display(Name = " صعودی")]
        Create_Date_Asc,
    }
}
