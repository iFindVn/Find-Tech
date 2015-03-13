using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using AutoMapper;
using FindTech.Entities.Models;
using FindTech.Entities.Models.Enums;
using FindTech.Services;
using FindTech.Web.Models;
using FindTech.Web.Models.Enums;
using Repository.Pattern.UnitOfWork;

namespace FindTech.Web.Controllers
{
    public class ArticleController : Controller
    {
        private IArticleService articleService { get; set; }
        private IContentSectionService contentSectionService { get; set; }
        private IOpinionService opinionService { get; set; }
        private IStoredProcedureService storedProcedureService { get; set; }
        private IUnitOfWorkAsync unitOfWork { get; set; }

        public ArticleController(IUnitOfWorkAsync unitOfWork, IArticleService articleService, IContentSectionService contentSectionService, IOpinionService opinionService, IStoredProcedureService storedProcedureService)
        {
            this.articleService = articleService;
            this.contentSectionService = contentSectionService;
            this.opinionService = opinionService;
            this.storedProcedureService = storedProcedureService;
            this.unitOfWork = unitOfWork;
        }
        // GET: Article
        //public ActionResult Detail(int id)
        //{
        //    var article = articleService.Queryable().Include(a => a.Source).Include(a => a.ArticleCategory).Include(a => a.ContentSections).FirstOrDefault(a => a.ArticleId == id);
        //    ViewBag.Article = Mapper.Map<ArticleViewModel>(article);
        //    return View();
        //}

        public ActionResult Detail(string seoTitle, int page = 1)
        {
            var article = articleService.GetArticleDetail(seoTitle);
            if (article == null) return null;
            article.ContentSections = contentSectionService.GetContentSections(article.ArticleId, page).ToList();
            var articleViewModel = Mapper.Map<ArticleViewModel>(article);
            var contentSectionPages = contentSectionService.GetContentSectionPages(article.ArticleId, page);
            ViewBag.ContentSectionPages = contentSectionPages.Select(a => new ContentSectionPageViewModel { IsCurrentPage = (bool)a.GetType().GetProperty("IsCurrentPage").GetValue(a), PageName = (string)a.GetType().GetProperty("PageName").GetValue(a), PageNumber = (int)a.GetType().GetProperty("PageNumber").GetValue(a) }).ToList();
            var articles = articleService.GetLatestReviews(0, 4);
            ViewBag.Articles = articles.Select(Mapper.Map<ArticleViewModel>).ToList();
            article.ViewCount++;
            articleService.Update(article);
            unitOfWork.SaveChangesAsync();
            return View(articleViewModel);
        }

        public ActionResult _NewsBoxs(int skip = 0, int take = 20)
        {
            var articles = articleService.Queryable().Where(a => a.ArticleType == ArticleType.News && a.IsHot != true).OrderByDescending(a => a.Priority).ThenByDescending(a => a.PublishedDate).Skip(skip).Take(take).Include(a => a.Source).Include(a => a.ArticleCategory).Include(a => a.ContentSections);
            return View(new ArticleListViewModel { Articles = articles.Select(Mapper.Map<ArticleViewModel>).ToList() });
        }

        public ActionResult _Widget(SearchType searchType, string keyword, ArticleListViewModel articleListViewModel)
        {
            var query = articleService.Queryable();
            switch (searchType)
            {
                case SearchType.Category:
                    query = query.Where(a => a.ArticleCategory.SeoName == keyword && a.IsHot != true);
                    break;
                case SearchType.Tags:
                    query = query.Where(a => a.Tags.Contains(keyword) && a.IsHot != true);
                    break;
                default: break;
            }
            var articles = query.OrderByDescending(a => a.Priority).ThenByDescending(a => a.PublishedDate).Skip(0).Take(20);
            articleListViewModel.Articles = articles.Select(Mapper.Map<ArticleViewModel>).ToList();
            return View(articleListViewModel);
        }

        public ActionResult _Pinned(ArticleListViewModel articleListViewModel)
        {
            if (Session["Pinned"] == null)
            {
                Session["Pinned"] = new List<ArticleViewModel>();
            }
            articleListViewModel.Articles = (List<ArticleViewModel>) Session["Pinned"];
            return View(articleListViewModel);
        }

        [HttpPost]
        public ActionResult Pin(int articleId)
        {
            var article = Mapper.Map<ArticleViewModel>(articleService.Find(articleId));
            var pinnedArticles = (List<ArticleViewModel>)Session["Pinned"];
            if (pinnedArticles.Select(a => a.ArticleId).Contains(articleId))
            {
                pinnedArticles.Remove(article);
            }
            else
            {
                pinnedArticles.Add(article);
            }
            return PartialView("_ListItemArticleBox", article);
        }

        [HttpPost]
        public ActionResult RateOpinion(int articleId, OpinionLevel opinionLevel)
        {
            var opinion = opinionService.GetOpinion(articleId, opinionLevel);
            if (opinion == null)
            {
                opinionService.Insert(new Opinion { OpinionCount = 1, OpinionLevel = opinionLevel, ArticleId = articleId });
            }
            else
            {
                opinion.OpinionCount++;
                opinionService.Update(opinion);
            }
            unitOfWork.SaveChanges();
            Session["RatedOpinion"] = true;
            return PartialView("_OpinionPanel", opinionService.GetOpinions(articleId).ToList().Select(Mapper.Map<OpinionViewModel>));
        }

        public ActionResult SearchArticles(string keyword)
        {
            var articles = storedProcedureService.SearchArticles(keyword);
            return Json(articles.ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}