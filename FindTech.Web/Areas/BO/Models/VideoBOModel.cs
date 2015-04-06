using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindTech.Web.Areas.BO.Models
{
    public class VideoBOModel
    {
        public int VideoId { get; set; }
        public string VideoName { get; set; }
        public string Thumbnail { get; set; }
        public string Url { get; set; }
    }
}