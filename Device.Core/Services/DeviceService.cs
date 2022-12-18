using M = Device.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Device.Core.Repository;
using System.Xml.Linq;
using Device.Core.Models;

namespace Device.Core.Services
{

    public interface IDeviceService
    {

        Task<M.Device> Create(M.Device device);
        Task<M.Device> Get(int id);
        Task<IEnumerable<M.Device>> GetAll();
        Task<M.Device> Update(M.Device device);
        Task<bool> Delete(int id);

        Task<IEnumerable<M.Device>> GetByBrand(string brand);

    }

    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository _deviceRepository;

        public DeviceService(IDeviceRepository deviceRepository)
            => this._deviceRepository = deviceRepository;
        public Task<M.Device> Create(M.Device device)
            => _deviceRepository.Create(device);

        public Task<M.Device> Get(int id)
            => _deviceRepository.Get(id);
        public Task<IEnumerable<M.Device>> GetAll()
            => _deviceRepository.GetAll();
        public Task<M.Device> Update(M.Device device)
            => _deviceRepository.Update(device);
        public Task<bool> Delete(int id)
            => _deviceRepository.Delete(id);

        public Task<IEnumerable<M.Device>> GetByBrand(string brand)
            => _deviceRepository.GetByBrand(brand);

    }
}
