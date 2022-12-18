using M = Device.Core.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Device.Core.Repository
{
    public interface IDeviceRepository
    {
        Task<M.Device> Create( M.Device device);
        Task<M.Device> Get(int id );
        Task<IEnumerable<M.Device>> GetAll();
        Task<M.Device> Update( M.Device device );
        Task<bool> Delete(int id );

        Task<IEnumerable<M.Device>> GetByBrand( string brand );

    }
}
