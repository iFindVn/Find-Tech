using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using FindTech.Entities.Models;
using FindTech.Entities.Models.Enums;
using Repository.Pattern.Repositories;

namespace FindTech.Repository.Repositories
{
    public static class DeviceColorsRepository
    {

        public static IEnumerable<DeviceColor> GetDeviceColors(this IRepositoryAsync<DeviceColor> deviceColorsRepository, int deviceId)
        {
            return deviceColorsRepository.Queryable().Include(a => a.DeviceImages)
                .Where(a => a.DeviceId == deviceId )
                .AsEnumerable();
        }
    }
}
