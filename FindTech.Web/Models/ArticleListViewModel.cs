using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FindTech.Web.Models.Enums;

namespace FindTech.Web.Models
{
    public class ArticleListViewModel
    {
        public string Title { get; set; }
        public string TitleStyleClass { get; set; }
        public WidgetType WidgetType { get; set; }
        public IEnumerable<ArticleViewModel> Articles { get; set; }
        public string ClientId { get; set; }
    }
}