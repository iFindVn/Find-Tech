using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using FindTech.Entities.Models;
using FindTech.Repository.Repositories;
using Repository.Pattern.Repositories;
using Service.Pattern;

namespace FindTech.Services
{

    public interface IDeviceColorsService : IService<DeviceColor>
    {
        IEnumerable<DeviceColor> GetDeviceColors(int deviceId);

    }

    class DeviceColorsService : Service<DeviceColor>, IDeviceColorsService
    {
         private readonly IRepositoryAsync<DeviceColor> _deviceColorsRepository;
         public DeviceColorsService(IRepositoryAsync<DeviceColor> deviceColorsRepository)
             : base(deviceColorsRepository)
        {
            _deviceColorsRepository = deviceColorsRepository;
        }


         public IEnumerable<DeviceColor> GetDeviceColors(int deviceId)
         {
             return _deviceColorsRepository.GetDeviceColors(deviceId);
         }
    }
}
