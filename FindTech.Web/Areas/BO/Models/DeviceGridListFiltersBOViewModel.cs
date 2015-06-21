using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindTech.Web.Areas.BO.Models
{
    public class DeviceGridListFiltersBOViewModel
    {
        public string Logic { get; set; }
        public List<DeviceGridFilterBOViewModel> Filters { get; set; }
    }
}