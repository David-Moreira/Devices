using Microsoft.EntityFrameworkCore;

namespace Device.Infrastructure
{
    public class DeviceContext : DbContext
    {
        public DeviceContext()
        {
        }

        public DeviceContext(DbContextOptions<DeviceContext> options) : base(options)
        {
        }

        public DbSet<Device.Core.Models.Device> Devices { get; set; }
    }
}