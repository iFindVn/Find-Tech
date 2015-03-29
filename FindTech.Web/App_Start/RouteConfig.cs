using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FindTech.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Search",
                url: "tim-kiem/{keyword}",
                defaults: new { controller = "Article", action = "Search", keyword = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "SeoTitle",
                url: "bai-viet/{categorySeoName}/{seoTitle}/{page}",
                defaults: new { controller = "Article", action = "Detail", categorySeoName = UrlParameter.Optional, seoTitle = UrlParameter.Optional, page = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
