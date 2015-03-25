using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Mvc;
using System.Linq;
using AutoMapper;
using FindTech.Entities.Models.Enums;
using FindTech.Entities.StoredProcedures.Models;
using FindTech.Services;
using FindTech.Web.Models;
using Repository.Pattern.UnitOfWork;

namespace FindTech.Web.Controllers
{
    public class HomeController : Controller
    {
        private IArticleService articleService { get; set; }
        private IUnitOfWorkAsync unitOfWork { get; set; }

        public HomeController(IUnitOfWorkAsync unitOfWork, IArticleService articleService)
        {
            this.articleService = articleService;
            this.unitOfWork = unitOfWork;
        }
        public ActionResult Index()
        {
            //ViewBag.Title =
            //    "Tìm là thấy";
            //ViewBag.Description = "Cổng thông tin công nghệ, thiết bị di động, so sánh sản phẩm công nghệ, đánh giá smart phone, tablet,...";
            //var hotArticles = articleService.GetHotArticles().Select(Mapper.Map<ArticleViewModel>);
            //ViewBag.HotArticles = hotArticles;
            //var latestReviews = articleService.GetLatestReviews().Select(Mapper.Map<ArticleViewModel>);
            //ViewBag.LatestReviews = latestReviews;
            //var popularReviews = articleService.GetHotReviews(0, 4).Select(Mapper.Map<ArticleViewModel>);
            //ViewBag.PopularReviews = popularReviews;
            return View();
        }

        public ActionResult Init()
        {
            var hotArticles = articleService.GetHotArticles(0, 10).Select(Mapper.Map<ArticleViewModel>);
            var latestReviews = articleService.GetLatestReviews(0, 10).Select(Mapper.Map<ArticleViewModel>);
            var latestNewses = articleService.GetLatestNewses(0, 20).Select(Mapper.Map<ArticleViewModel>);
            var trickAndTipArticles = articleService.GetListOfArticles(new GetListOfArticlesParameters
            {
                ArticleType = ArticleType.All,
                Categories = "thu-thuat-va-meo-vat",
                Tags = "",
                OrderString = "",
                Skip = 0,
                Take = 10,
                WhereClauseMore = "",
                SkipArticleIds = ""
            }).Select(Mapper.Map<ArticleViewModel>);
            var appAndGameArticles = articleService.GetListOfArticles(new GetListOfArticlesParameters
            {
                ArticleType = ArticleType.All,
                Categories = "ung-dung-va-game",
                Tags = "",
                OrderString = "",
                Skip = 0,
                Take = 10,
                WhereClauseMore = "",
                SkipArticleIds = ""
            }).Select(Mapper.Map<ArticleViewModel>);
            var productAndTechToyArticles = articleService.GetListOfArticles(new GetListOfArticlesParameters
            {
                ArticleType = ArticleType.All,
                Categories = "san-pham,do-choi-cong-nghe",
                Tags = "",
                OrderString = "",
                Skip = 0,
                Take = 10,
                WhereClauseMore = "",
                SkipArticleIds = ""
            }).Select(Mapper.Map<ArticleViewModel>);
            var brandAndDigiLifeArticles = articleService.GetListOfArticles(new GetListOfArticlesParameters
            {
                ArticleType = ArticleType.All,
                Categories = "thuong-hieu,doi-song-so",
                Tags = "",
                OrderString = "",
                Skip = 0,
                Take = 10,
                WhereClauseMore = "",
                SkipArticleIds = ""
            }).Select(Mapper.Map<ArticleViewModel>);
            return Json(new { hotArticles, latestReviews, trickAndTipArticles, appAndGameArticles, productAndTechToyArticles, brandAndDigiLifeArticles, latestNewses }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult UnderConstruction()
        {
            return View();
        }
    }
}