using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindTech.Web.Areas.BO.Models
{
    public class SpecDetailBOViewModel
    {
        public int SpecDetailId { get; set; }
        public string Value { get; set; }
        public bool HighLight { get; set; }
        public SpecBOViewModel Spec { get; set; }
        public DeviceBOViewModel Device { get; set; }
    }
}