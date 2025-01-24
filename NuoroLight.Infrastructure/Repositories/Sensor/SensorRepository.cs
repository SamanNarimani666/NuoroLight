using Microsoft.EntityFrameworkCore;
using NuoroLight.Domain.Entities.Device;
using NuoroLight.Domain.IRepositories;
using NuoroLight.Infrastructure.Context;
using System.Linq.Expressions;

namespace NuoroLight.Infrastructure.Repositories.Sensor
{
    public class SensorRepository : ISensorRepository
    {
        private readonly NuoroLightContext _nuoroLightContext;

        public SensorRepository(NuoroLightContext nuoroLightContext)
        {
            this._nuoroLightContext = nuoroLightContext;
        }

        public async Task Add(Domain.Entities.Device.Sensor sensor)
        {
            await _nuoroLightContext.AddAsync(sensor);
        }

        public async Task AddCommands(List<Command> commands)
        {
            await _nuoroLightContext.Commands.AddRangeAsync(commands);  
        }

        public async Task<bool> AnyAsync(Expression<Func<Domain.Entities.Device.Sensor, bool>> predicate)
        {
            return await _nuoroLightContext.Sensors.AnyAsync(predicate);
        }

        public async ValueTask DisposeAsync()
        {
            if (_nuoroLightContext != null)
                await _nuoroLightContext.DisposeAsync();
        }

        public async Task SaveChangeAsync()
        {
            await _nuoroLightContext.SaveChangesAsync();
        }
    }
}
