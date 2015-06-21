using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FindTech.Entities.Models.Enums;
using FindTech.Web.Areas.BO.Models.Common;

namespace FindTech.Web.Areas.BO.Models
{
    public class DeviceGridBOViewModel
    {
        public int DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
        public MarketStatusDropDown MarketStatus { get; set; }
        public DateTime? ReleasedDate { get; set; }
        public int ViewCount { get; set; }
        public BoxSizeDropDown BoxSize { get; set; }
        public int Priority { get; set; }
        public int Brand { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int Article_ArticleId { get; set; }
        public bool IsHot { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
       
        
    }

    public class MarketStatusDropDown
    {
        public int MarketStatusId { get; set; }
        public String MarketStatusName { get; set; }
    }
}