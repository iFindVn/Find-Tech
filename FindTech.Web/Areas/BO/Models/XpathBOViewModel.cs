using System;
using FindTech.Entities.Models.Enums;

namespace FindTech.Web.Areas.BO.Models
{
    public class XpathBOViewModel
    {
        public int XpathId { get; set; }
        public string XpathString { get; set; }
        public ArticleField ArticleField { get; set; }
        public int SourceId { get; set; }
    }
}