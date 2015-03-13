using System;
using FindTech.Entities.Models.Enums;

namespace FindTech.Web.Areas.BO.Models
{
    public class BrandBOViewModel
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string Logo { get; set; }
        public int Priority { get; set; }
        public BrandType BrandType { get; set; }
    }
}