using System;
using FindTech.Entities.Models.Enums;

namespace FindTech.Web.Areas.BO.Models
{
    public class SpecBOViewModel
    {
        public int SpecId { get; set; }
        public string SpecName { get; set; }
        public int Priority { get; set; }
        public bool? IsMain { get; set; }
        public SpecGroupBOViewModel SpecGroup { get; set; }
    }
}