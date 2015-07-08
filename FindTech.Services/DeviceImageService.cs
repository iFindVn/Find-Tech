using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FindTech.Entities.Models;
using Repository.Pattern.Repositories;
using Service.Pattern;

namespace FindTech.Services
{


    public interface IDeviceImageService : IService<DeviceImage>
    {
    }

    public class DeviceImageService : Service<DeviceImage>, IDeviceImageService
    {
        public DeviceImageService(IRepositoryAsync<DeviceImage> deViceImageRepository)
            : base(deViceImageRepository)
        {
        }
    }

}
