using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FindTech.Entities.Models;

namespace FindTech.Web.Areas.BO.Models
{
    public class DeviceColorsBOViewModel
    {
        public DeviceColorsBOViewModel()
        {
            DeviceImages = new List<DeviceImage>();
        }
        public int DeviceColorId { get; set; }
        public string DeviceColorName { get; set; }
        public string DeviceColorCode { get; set; }
        public string Color { get; set; }
        public int DeviceId { get; set; }
        public virtual DeviceBOViewModel Device { get; set; }
        public virtual ICollection<DeviceImage> DeviceImages { get; set; } 
        
    }
}