using Device.Core.Repository;

using Microsoft.EntityFrameworkCore;

using M = Device.Core.Models;

namespace Device.Infrastructure.Repository
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly DeviceContext _dbContext;

        public DeviceRepository(DeviceContext dbContext)
            => _dbContext = dbContext;

        public async Task<M.Device> Create(M.Device device)
        {
            device.CreatedOn = DateTime.Now;
            await _dbContext.AddAsync(device);
            await _dbContext.SaveChangesAsync();

            return device;
        }

        public Task<M.Device> Get(int id)
            => _dbContext.Set<M.Device>().FindAsync(id).AsTask();

        public async Task<IEnumerable<M.Device>> GetAll()
            => await _dbContext.Set<M.Device>().ToListAsync();

        public async Task<M.Device> Update(M.Device device)
        {
            var entity = await _dbContext.Set<M.Device>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == device.Id);
            if (entity is null)
                return null;

            _dbContext.Update(device);
            await _dbContext.SaveChangesAsync();

            return device;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await Get(id);
            if (entity is null)
                return false;

            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<M.Device>> GetByBrand(string brand)
            => await _dbContext.Set<M.Device>().Where(x => x.Brand == brand).ToListAsync();
    }
}