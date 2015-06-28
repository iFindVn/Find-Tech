using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindTech.Web.Areas.BO.Models
{
    public class SpecDetailGridBOViewModel
    {
        public int SpecDetailId { get; set; }
        public int SpecId { get; set; }
        public string SpecName { get; set; }
        public string Value { get; set; }
        public bool HighLight { get; set; }
        public int SpecGroupId { get; set; }
        public string SpecGroupName { get; set; }
    }
}