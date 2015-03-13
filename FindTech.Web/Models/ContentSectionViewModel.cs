using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindTech.Web.Models
{
    public class ContentSectionViewModel
    {
        public int ContentSectionId { get; set; }
        public string SectionTitle { get; set; }
        public string SectionDescription { get; set; }
        public string SectionContent { get; set; }
        public int PageNumber { get; set; }
        public int BenchmarkId { get; set; }
    }
}