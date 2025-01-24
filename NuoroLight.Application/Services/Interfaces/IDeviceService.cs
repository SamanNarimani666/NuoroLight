using NuoroLight.Domain.Entities.Device;
using NuoroLight.Domain.ViewModels.Device;
using NuoroLight.Domain.ViewModels.Sensor;

namespace NuoroLight.Application.Services.Interfaces;
public interface IDeviceService : IAsyncDisposable
{
    Task<FilterDeviceViewModel> FilterDevice(FilterDeviceViewModel filter);

    Task<CreateDeviceResult> CreateDevice(CreateDeviceViewModel createDevice);

    Task<List<Sensor>> GetDeviceSesnsors(int deviceId);

    Task<CreateSensorResult> CreateSensor(CreateSensorViewModel createSensor);
    Task AddLogs(string text);

    Task<FilterDeviceLogsViewModel> FilterDeviceLogs(FilterDeviceLogsViewModel filter);

}
