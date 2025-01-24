using NuoroLight.Domain.Entities.Device;
using NuoroLight.Domain.ViewModels.Device;
using System.Linq.Expressions;

namespace NuoroLight.Domain.IRepositories;
public interface IDeviceRepository : IAsyncDisposable
{
    Task AddDevice(Device device);

    void EditDevice(Device device);

    Task Save();

    Task<bool> AnyAsync(Expression<Func<Device, bool>> predicate);

    Task<Device> FirstOrDefaultAsync(Expression<Func<Device, bool>> predicate);
    Task<Device> GetById(int deviceId);

    Task<FilterDeviceViewModel> FilterDevice(FilterDeviceViewModel filter);
    Task<List<Sensor>> GetDeviceSesnsors(int deviceId);
    Task AddLogs(string text);
    Task<FilterDeviceLogsViewModel> FilterDeviceLogs(FilterDeviceLogsViewModel filter);

}
