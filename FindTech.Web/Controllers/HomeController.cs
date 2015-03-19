using System.Data.Entity;
using System.Web.Mvc;
using System.Linq;
using AutoMapper;
using FindTech.Entities.Models.Enums;
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