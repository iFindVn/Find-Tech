using FindTech.Entities.Models.Enums;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindTech.Entities.Models
{
    public class DeviceImage: Entity
    {
        public int DeviceImageId { get; set; }
        public string DeviceImageName { get; set; }
        public string Thumbnail { get; set; }
        public string Url { get; set; }
        public ImageType ImageType { get; set; }
        public int DeviceColorId { get; set; }
        public virtual DeviceColor DeviceColor { get; set; }
    }
}
