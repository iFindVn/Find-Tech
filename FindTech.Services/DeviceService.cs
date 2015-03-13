using FindTech.Entities.Models;
using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindTech.Services
{

    public interface IDeviceService : IService<Device>
    {
    }
    class DeviceService: Service<Device>, IDeviceService
    {
        public DeviceService(IRepositoryAsync<Device> deviceRepository)
            : base(deviceRepository)
        {
        }
    }
}
