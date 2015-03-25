using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using AutoMapper;
using FindTech.Entities.Models;
using FindTech.Entities.Models.Enums;
using FindTech.Entities.StoredProcedures.Models;
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
            ViewBag.CurrentPage = page;
            ViewBag.SeoTitle = seoTitle;
            return View();
        }

        public ActionResult GetArticleDetail(string seoTitle, int page = 1)
        {
            var article = articleService.GetArticleDetail(seoTitle);
            if (article == null) return null;
            article.ContentSections = contentSectionService.GetContentSections(article.ArticleId, page).ToList();
            article.ViewCount++;
            articleService.Update(article);
            unitOfWork.SaveChanges();

            var contentSectionPages = contentSectionService.GetContentSectionPages(article.ArticleId, page).Select(
                        a =>
                            new ContentSectionPageViewModel
                            {
                                IsCurrentPage = (bool)a.GetType().GetProperty("IsCurrentPage").GetValue(a),
                                PageName = (string)a.GetType().GetProperty("PageName").GetValue(a),
                                PageNumber = (int)a.GetType().GetProperty("PageNumber").GetValue(a)
                            }).ToList();
            var contentSectionPageManager = contentSectionPages.Count > 0 ? new
                {
                    contentSectionPages,
                    currentPage = contentSectionPages.FirstOrDefault(a => a.PageNumber == page),
                    nextPage = contentSectionPages.FirstOrDefault(a => a.PageNumber == page + 1),
                    minPageNumber = contentSectionPages.Min(a => a.PageNumber)
                } : new
                {
                    contentSectionPages,
                    currentPage = new ContentSectionPageViewModel(),
                    nextPage = new ContentSectionPageViewModel(),
                    minPageNumber = 0
                };
            var sameCategoryNewses = articleService.GetListOfArticles(new GetListOfArticlesParameters
            {
                ArticleType = ArticleType.News,
                Categories = article.ArticleCategory.SeoName,
                Tags = "",
                OrderString = "",
                Skip = 0,
                Take = 4,
                WhereClauseMore = "",
                SkipArticleIds = article.ArticleId.ToString()
            }).Select(Mapper.Map<ArticleViewModel>);

            var relatedNewses = articleService.GetListOfArticles(new GetListOfArticlesParameters
            {
                ArticleType = ArticleType.News,
                Categories = "",
                Tags = article.Tags,
                OrderString = "",
                Skip = 0,
                Take = 10,
                WhereClauseMore = "",
                SkipArticleIds = article.ArticleId.ToString()
            }).Select(Mapper.Map<ArticleViewModel>);

            var hotNewses = articleService.GetHotNewses(0, 4, article.ArticleId.ToString()).Select(Mapper.Map<ArticleViewModel>);

            if (Session["LikedCommentIds"] == null)
            {
                Session["LikedCommentIds"] = new List<int>();
            }
            var likedCommentIds = (List<int>)Session["LikedCommentIds"];
            return Json(new { article = Mapper.Map<ArticleViewModel>(article), contentSectionPageManager, sameCategoryNewses, relatedNewses, hotNewses, likedCommentIds }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetContentSectionPages(int articleId, int page)
        {
            var contentSectionPages = contentSectionService.GetContentSectionPages(articleId, page).Select(
                        a =>
                            new ContentSectionPageViewModel
                            {
                                IsCurrentPage = (bool) a.GetType().GetProperty("IsCurrentPage").GetValue(a),
                                PageName = (string) a.GetType().GetProperty("PageName").GetValue(a),
                                PageNumber = (int) a.GetType().GetProperty("PageNumber").GetValue(a)
                            }).ToList();
            if (contentSectionPages.Count > 0)
            {
                var currentPage = contentSectionPages.FirstOrDefault(a => a.PageNumber == page);
                var nextPage = contentSectionPages.FirstOrDefault(a => a.PageNumber == page + 1);
                var minPageNumber = contentSectionPages.Min(a => a.PageNumber);
                return
                    Json(
                        new { contentSectionPages, currentPage, nextPage, minPageNumber }, JsonRequestBehavior.AllowGet);
            }
            return Json(
                        new { contentSectionPages }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLatestReviews(int skip, int take)
        {
            var articles = articleService.GetLatestReviews(skip, take);
            return
                Json(
                    new ArticleListViewModel
                    {
                        Articles = articles.Select(Mapper.Map<ArticleViewModel>),
                        Title = "Soi mới nhất",
                        TitleStyleClass = "fa fa-eye background-info"
                    }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLatestNewses(int skip, int take)
        {
            var articles = articleService.GetLatestNewses(skip, take);
            return
                Json(articles.Select(Mapper.Map<ArticleViewModel>), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetHotReviews(int skip, int take)
        {
            var articles = articleService.GetHotReviews(skip, take);
            return
                 Json(
                     new ArticleListViewModel
                     {
                         Articles = articles.Select(Mapper.Map<ArticleViewModel>),
                         Title = "Soi nóng nhất",
                         TitleStyleClass = "fa fa-fire background-danger"
                     }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetHotNewses(int skip, int take)
        {
            var articles = articleService.GetHotNewses(skip, take); 
            return
                 Json(
                     new ArticleListViewModel
                     {
                         Articles = articles.Select(Mapper.Map<ArticleViewModel>),
                         Title = "Tin nóng",
                         TitleStyleClass = "fa fa-fire background-danger"
                     }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetHotArticles(int skip, int take)
        {
            var articles = articleService.GetHotArticles(skip, take);
            return
                 Json(
                     new ArticleListViewModel
                     {
                         Articles = articles.Select(Mapper.Map<ArticleViewModel>),
                         Title = "Bài nóng nhất",
                         TitleStyleClass = "fa fa-fire background-danger"
                     }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetArticleByCatogories(string categories, ArticleType articleType, string articleId, int skip, int take)
        {
            var articles = articleService.GetListOfArticles(new GetListOfArticlesParameters
            {
                ArticleType = articleType,
                Categories = categories,
                Tags = "",
                OrderString = "",
                Skip = skip,
                Take = take,
                WhereClauseMore = "",
                SkipArticleIds = articleId
            });
            return
                 Json(
                     new ArticleListViewModel
                     {
                         Articles = articles.Select(Mapper.Map<ArticleViewModel>),
                         Title = "Tin cùng chuyên mục",
                         TitleStyleClass = "fa fa-folder-open background-success"
                     }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetArticleByTags(string tags, ArticleType articleType, string articleId, int skip, int take)
        {
            var articles = articleService.GetListOfArticles(new GetListOfArticlesParameters
            {
                ArticleType = articleType,
                Categories = "",
                Tags = tags,
                OrderString = "",
                Skip = skip,
                Take = take,
                WhereClauseMore = "",
                SkipArticleIds = articleId
            });
            return
                 Json(
                     new ArticleListViewModel
                     {
                         Articles = articles.Select(Mapper.Map<ArticleViewModel>),
                         Title = "Tin liên quan",
                         TitleStyleClass = "fa fa-tag background-primary"
                     }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPinnedArticles()
        {
            if (Session["Pinned"] == null)
            {
                Session["Pinned"] = new List<ArticleViewModel>();
            }
            return
                Json((List<ArticleViewModel>)Session["Pinned"], JsonRequestBehavior.AllowGet);
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
            var pinnedArticles = (List<ArticleViewModel>)Session["Pinned"];
            var existedArticle = pinnedArticles.FirstOrDefault(a => a.ArticleId == articleId);
            if (existedArticle != null)
            {
                pinnedArticles.Remove(existedArticle);
            }
            else
            {
                var article = Mapper.Map<ArticleViewModel>(articleService.Find(articleId));
                pinnedArticles.Insert(0, article);
            }
            return Json(pinnedArticles, JsonRequestBehavior.AllowGet);
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
            return Json(opinionService.GetOpinions(articleId).ToList().Select(Mapper.Map<OpinionViewModel>));
        }

        public ActionResult SearchArticles(string keyword)
        {
            var articles = storedProcedureService.SearchArticles(keyword);
            return Json(articles.ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}