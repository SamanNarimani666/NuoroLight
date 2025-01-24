using NuoroLight.Application.Services.Interfaces;
using NuoroLight.Domain.Entities.Device;
using NuoroLight.Domain.IRepositories;
using NuoroLight.Domain.ViewModels.Device;
using NuoroLight.Domain.ViewModels.Sensor;

namespace NuoroLight.Application.Services.Implementations;
public class DeviceService : IDeviceService
{
    #region Filed
    private readonly IDeviceRepository _deviceRepository;
    private readonly ISensorRepository _sensorRepository;
    #endregion

    #region Constructor
    public DeviceService(IDeviceRepository deviceRepository, ISensorRepository sensorRepository)
    {
        _deviceRepository = deviceRepository;
        _sensorRepository = sensorRepository;
    }
    #endregion

    #region CreateDevice
    public async Task<CreateDeviceResult> CreateDevice(CreateDeviceViewModel createDevice)
    {
        try
        {
            if (await _deviceRepository.AnyAsync(d => d.Name.Trim() == createDevice.Name.Trim() && !d.IsDelete))
                return CreateDeviceResult.ExistsDeviceName;


            if (await _deviceRepository.AnyAsync(d => d.Name.Trim() == createDevice.MAC.Trim() && !d.IsDelete))
                return CreateDeviceResult.ExistsDeviceMAC;


            await _deviceRepository.AddDevice(new Device
            {
                IsActive = createDevice.IsActive,
                Name = createDevice.Name,
                CreatedAt = DateTime.Now,
                IsDelete = false,
                Location = createDevice.Location,
                MAC = createDevice.MAC
            });
            await _deviceRepository.Save();
            return CreateDeviceResult.Success;
        }
        catch
        {
            return CreateDeviceResult.Error;
        }
    }
    #endregion

    #region DisposeAsync
    public async ValueTask DisposeAsync()
    {
        await _deviceRepository.DisposeAsync();
    }

    #endregion

    #region FilterDevice
    public async Task<FilterDeviceViewModel> FilterDevice(FilterDeviceViewModel filter)
    {
        return await _deviceRepository.FilterDevice(filter);
    }
    #endregion

    #region GetDeviceSesnsors
    public async Task<List<Sensor>> GetDeviceSesnsors(int deviceId)
    {
        return await _deviceRepository.GetDeviceSesnsors(deviceId);
    }
    #endregion

    #region CreateSensor
    public async Task<CreateSensorResult> CreateSensor(CreateSensorViewModel createSensor)
    {
        try
        {
            if (createSensor.Commands.Count < 0)
                return CreateSensorResult.EmptyCommands;

            var sensor = new Sensor
            {
                Name = createSensor.Name,
                DeviceId = createSensor.DeviceId,
                IsDelete = false,
                SensorType = createSensor.SensorType,
            };

            await _sensorRepository.Add(sensor);
            await _sensorRepository.SaveChangeAsync();

            List<Command> commands = new List<Command>();
            foreach (var item in createSensor.Commands)
            {
                commands.Add(new Command
                {
                    Description = item.Description,
                    Display = item.Display,
                    Value = item.Value,
                    SensorId = sensor.SensorId
                });
            }

            await _sensorRepository.AddCommands(commands);
            await _sensorRepository.SaveChangeAsync();

            return CreateSensorResult.Success;
        }
        catch (Exception)
        {
            return CreateSensorResult.Error;
        }
    }
    #endregion

    #region AddLogs
    public async Task AddLogs(string text)
    {
        try
        {
            await _deviceRepository.AddLogs(text);
            await _deviceRepository.Save();
        }
        catch { }
    }
    #endregion

    #region FilterDeviceLogs
    public async Task<FilterDeviceLogsViewModel> FilterDeviceLogs(FilterDeviceLogsViewModel filter)
    {
        return await _deviceRepository.FilterDeviceLogs(filter);
    }

    #endregion
}
