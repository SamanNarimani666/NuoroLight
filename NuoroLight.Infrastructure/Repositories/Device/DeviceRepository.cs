using Microsoft.EntityFrameworkCore;
using NuoroLight.Domain.Entities.Device;
using NuoroLight.Domain.Entities.User;
using NuoroLight.Domain.IRepositories;
using NuoroLight.Domain.ViewModels.Device;
using NuoroLight.Domain.ViewModels.Paging;
using NuoroLight.Domain.ViewModels.User;
using NuoroLight.Infrastructure.Context;
using System.Linq.Expressions;

namespace NuoroLight.Infrastructure.Repositories.Device
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly NuoroLightContext _context;

        public DeviceRepository(NuoroLightContext context)
        {
            _context = context;
        }

        public async Task AddDevice(Domain.Entities.Device.Device device) => await _context.Devices.AddAsync(device);

        public async Task AddLogs(string text)
        {
            await _context.DeviceLogs.AddAsync(new DeviceLog
            {
                CreatedAt = DateTime.Now,
                Text = text
            });
        }

        public async Task<bool> AnyAsync(Expression<Func<Domain.Entities.Device.Device, bool>> predicate) => await _context.Devices.AnyAsync(predicate);

        #region DisposeAsync
        public async ValueTask DisposeAsync()
        {
            if (_context != null)
            {
                await _context.DisposeAsync();
            }
        }
        #endregion


        public void EditDevice(Domain.Entities.Device.Device device) => _context.Devices.Update(device);

        public async Task<FilterDeviceViewModel> FilterDevice(FilterDeviceViewModel filter)
        {
            var query = _context.Devices.Where(c => !c.IsDelete).AsQueryable();

            #region Order
            switch (filter.DeviceOrderBy)
            {
                case FilterDeviceOrderBy.Create_Date_Asc:
                    query = query.OrderBy(p => p.CreatedAt);
                    break;

                case FilterDeviceOrderBy.Create_Date_Desc:
                    query = query.OrderByDescending(p => p.CreatedAt);
                    break;
            }
            #endregion

            if (!string.IsNullOrEmpty(filter.Name))
                query = query.Where(c => EF.Functions.Like(c.Name, $"%{filter.Name}%"));

            #region Paging
            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity,
                filter.HowManyShowPageAfterAndBefore);
            var allProduct = query.Paging(pager).ToList();
            return filter.SetPaging(pager).SetUser(allProduct);

            #endregion
        }

        public async Task<FilterDeviceLogsViewModel> FilterDeviceLogs(FilterDeviceLogsViewModel filter)
        {
           var query = _context.DeviceLogs.AsQueryable();


            switch (filter.OrderBy)
            {
                case FilterDeviceLogOrderBy.Create_Date_Desc:
                    query = query.OrderByDescending(c => c.CreatedAt);
                    break;
                case FilterDeviceLogOrderBy.Create_Date_Asc:
                    query = query.OrderBy(c => c.CreatedAt);
                    break;
            }

            #region Paging
            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity,
                filter.HowManyShowPageAfterAndBefore);
            var allProduct = query.Paging(pager).ToList();
            return filter.SetPaging(pager).SetUser(allProduct);

            #endregion
        }

        public async Task<Domain.Entities.Device.Device> FirstOrDefaultAsync(Expression<Func<Domain.Entities.Device.Device, bool>> predicate) => await _context.Devices.FirstOrDefaultAsync(predicate);


        public async Task<Domain.Entities.Device.Device> GetById(int deviceId) => await _context.Devices.FindAsync(deviceId);

        public async Task<List<Domain.Entities.Device.Sensor >> GetDeviceSesnsors(int deviceId) => await _context.Sensors.Include(s=>s.Commands).Where(s=>s.DeviceId == deviceId).ToListAsync();

        public async Task Save() => await _context.SaveChangesAsync();
    }
}
