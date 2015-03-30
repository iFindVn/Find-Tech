using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FindTech.Entities.Models.Enums;

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
                name: "Categories",
                url: "danh-muc/{categories}",
                defaults: new { controller = "Article", action = "GetList", categories = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Tags",
                url: "nhan/{tags}",
                defaults: new { controller = "Article", action = "GetList", tags = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "HotNewses",
                url: "tin-nong",
                defaults: new { controller = "Article", action = "GetList", isHot = true, articleType = ArticleType.News }
            );

            routes.MapRoute(
                name: "HotReviews",
                url: "soi-nong",
                defaults: new { controller = "Article", action = "GetList", isHot = true, articleType = ArticleType.Reviews }
            );

            routes.MapRoute(
                name: "Newses",
                url: "tin-tuc",
                defaults: new { controller = "Article", action = "GetList", articleType = ArticleType.News }
            );

            routes.MapRoute(
                name: "Reviews",
                url: "soi",
                defaults: new { controller = "Article", action = "GetList", articleType = ArticleType.Reviews }
            );

            routes.MapRoute(
                name: "Pinned",
                url: "ghim",
                defaults: new { controller = "Article", action = "GetPinned" }
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
