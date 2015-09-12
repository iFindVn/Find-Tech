using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindTech.Web.Areas.BO.Models
{
    public class RivalProductsBOViewModel
    {
        public int DeviceHome { get; set; }
        public virtual DeviceBOViewModel DeviceHomeObj { get; set; }
        public int DeviceRival { get; set; }
        public virtual DeviceBOViewModel DeviceRivalObj { get; set; }
        public int Priority { get; set; }
        public bool? IsActived { get; set; }
        public bool? IsDeleted { get; set; }

    }
}