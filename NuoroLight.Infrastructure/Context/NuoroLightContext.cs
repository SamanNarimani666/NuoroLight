using Microsoft.EntityFrameworkCore;
using NuoroLight.Domain.Entities.Device;
using NuoroLight.Domain.Entities.Permissions;
using NuoroLight.Domain.Entities.SiteSetting;
using NuoroLight.Domain.Entities.User;

namespace NuoroLight.Infrastructure.Context
{
    public class NuoroLightContext : DbContext
    {
        #region Constructor
        public NuoroLightContext(DbContextOptions<NuoroLightContext> options) : base(options) { }
        #endregion

        #region Entities
        public DbSet<User> Users { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<SiteSetting> SiteSettings { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<Command> Commands { get; set; }
        public DbSet<DeviceLog> DeviceLogs { get; set; }

        #endregion

        #region OnModelCreating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email, "I_NC_Users_User_Email");

                entity.HasIndex(e => e.Mobile, "I_NC_Users_User_Mobile");

                entity.HasIndex(e => e.Password, "I_NC_Users_User_PassWord");

                entity.HasIndex(e => e.UserName, "I_NC_Users_User_UserName").IsUnique();

                entity.HasIndex(s => s.IsDelete, "I_NC_Users_User_IsDelete");

                entity.HasIndex(e => e.Email, "UQ_Users_User_Email")
                    .IsUnique();
            });

        }
        #endregion
    }
}
