using System;
using System.Collections.Generic;
using FindTech.Entities.Models.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FindTech.Web.Areas.BO.Models
{
    public class ArticleGridListFiltersBOViewModel
    {
        public string Logic { get; set; }
        public List<ArticleGridFilterBOViewModel> Filters { get; set; }
       
    }

   
}