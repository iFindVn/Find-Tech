using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindTech.Entities.Models
{
    class UserDevice
    {
        public int UserDeviceId { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public int DeviceId { get; set; }
        public virtual Device Device { get; set; }
        public int OwnerType { get; set; }
    }
}
