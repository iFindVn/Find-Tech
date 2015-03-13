using System;
using FindTech.Entities.Models.Enums;

namespace FindTech.Web.Areas.BO.Models
{
    public class SourceBOViewModel
    {
        public int SourceId { get; set; }
        public string SourceName { get; set; }
        public string Logo { get; set; }
        public string Link { get; set; }
    }
}