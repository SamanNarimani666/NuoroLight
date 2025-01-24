using NuoroLight.Domain.Entities.Device;
using System.Linq.Expressions;

namespace NuoroLight.Domain.IRepositories
{
    public interface ISensorRepository : IAsyncDisposable
    {
        Task Add(Sensor sensor);
        Task SaveChangeAsync();
        Task<bool> AnyAsync(Expression<Func<Sensor, bool>> predicate);

        Task AddCommands(List<Command> commands);
    }
}
