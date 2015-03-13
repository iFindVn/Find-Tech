using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindTech.Web.Areas.BO.Models
{
    public class ArticleGridFilterBOViewModel
    {
        public String Field  { get; set; }

        public String Operator { get; set; }

        public String Value { get; set; }
    }
}