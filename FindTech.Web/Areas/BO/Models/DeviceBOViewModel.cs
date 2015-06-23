using FindTech.Entities.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindTech.Web.Areas.BO.Models
{
    public class DeviceBOViewModel
    {
        public int DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
        public MarketStatus MarketStatus { get; set; }
        public DateTime? ReleasedDate { get; set; }
        public int ViewCount { get; set; }
        public BoxSize BoxSize { get; set; }
        public int BrandId { get; set; }
        public bool IsHot { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public int Priority { get; set; }
        public virtual BrandBOViewModel Brand { get; set; }
    }
}