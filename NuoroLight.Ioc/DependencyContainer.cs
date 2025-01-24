using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NuoroLight.Application.Security.PassWordHashing;
using NuoroLight.Application.Senders;
using NuoroLight.Application.Services.Implementations;
using NuoroLight.Application.Services.Interfaces;
using NuoroLight.Data.Repositories.Permission;
using NuoroLight.Data.Repositories.SiteSetting;
using NuoroLight.Data.Repositories.User;
using NuoroLight.Data.Repositories.UserPermission;
using NuoroLight.Domain.IRepositories;
using NuoroLight.Infrastructure.Context;
using NuoroLight.Infrastructure.Repositories.Device;
using NuoroLight.Infrastructure.Repositories.Sensor;
namespace NuoroLight.IOC;
public static class DependencyContainer
{
    public static void RegisterServices(IServiceCollection services, string connectionString)
    {
        #region DBContext
        services.AddDbContext<NuoroLightContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
        #endregion

        #region Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISiteSettingRepository, SiteSettingRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IUserPermissionRepository, UserPermissionRepository>();
        services.AddScoped<IDeviceRepository, DeviceRepository>();
        services.AddScoped<ISensorRepository, SensorRepository>();
        #endregion

        #region Tools
        services.AddSingleton<ISender, EmailSender>();
        services.AddSingleton<IPasswordHelper, PasswordHelper>();
        #endregion

        #region Services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ISiteService, SiteService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IDeviceService, DeviceService>();

        #endregion
    }
}

