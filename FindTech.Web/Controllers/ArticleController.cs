using System;
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
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Repository.Pattern.UnitOfWork;

namespace FindTech.Web.Controllers
{
    public class ArticleController : Controller
    {
        private IArticleService articleService { get; set; }
        private IContentSectionService contentSectionService { get; set; }
        private IArticleCategoryService articleCategoryService { get; set; }
        private IOpinionService opinionService { get; set; }
        private IStoredProcedureService storedProcedureService { get; set; }
        private IUnitOfWorkAsync unitOfWork { get; set; }

        public ArticleController(IUnitOfWorkAsync unitOfWork, IArticleService articleService, IContentSectionService contentSectionService, IArticleCategoryService articleCategoryService, IOpinionService opinionService, IStoredProcedureService storedProcedureService)
        {
            this.articleService = articleService;
            this.contentSectionService = contentSectionService;
            this.articleCategoryService = articleCategoryService;
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
            ViewBag.ContentSectionPageManager = JsonConvert.SerializeObject(contentSectionPageManager);
            if (article.ArticleType == ArticleType.News)
            {
                var sameCategoryNewses = articleService.GetListOfArticles(new GetListOfArticlesParameters
                {
                    ArticleType = ArticleType.News,
                    Categories = article.ArticleCategory.SeoName ?? "",
                    Tags = "",
                    OrderString = "",
                    Skip = 0,
                    Take = 4,
                    WhereClauseMore = "",
                    SkipArticleIds = article.ArticleId.ToString()
                }).Select(Mapper.Map<ArticleViewModel>);
                ViewBag.SameCategoryNewses = JsonConvert.SerializeObject(sameCategoryNewses);
                var hotNewses = articleService.GetHotNewses(0, 4, article.ArticleId.ToString()).Select(Mapper.Map<ArticleViewModel>);
                ViewBag.HotNewses = JsonConvert.SerializeObject(hotNewses);
            }
            else
            {
                ViewBag.SameCategoryNewses = JsonConvert.SerializeObject(new List<ArticleListViewModel>());
                ViewBag.HotNewses = JsonConvert.SerializeObject(new List<ArticleListViewModel>());
            }
            if (article.ArticleType == ArticleType.Reviews)
            {
                var latestReviews = articleService.GetLatestReviews(0, 4).Select(Mapper.Map<ArticleViewModel>);
                ViewBag.LatestReviews = JsonConvert.SerializeObject(latestReviews);
                var hotReviews = articleService.GetHotReviews(0, 4, "").Select(Mapper.Map<ArticleViewModel>);
                ViewBag.HotReviews = JsonConvert.SerializeObject(hotReviews);
                var relatedReviews = articleService.GetListOfArticles(new GetListOfArticlesParameters
                {
                    ArticleType = ArticleType.Reviews,
                    Categories = "",
                    Tags = article.Tags ?? "",
                    OrderString = "",
                    Skip = 0,
                    Take = 10,
                    WhereClauseMore = "",
                    SkipArticleIds = article.ArticleId.ToString()
                }).Select(Mapper.Map<ArticleViewModel>);
                ViewBag.RelatedReviews = JsonConvert.SerializeObject(relatedReviews);
            }
            else
            {
                ViewBag.LatestReviews = JsonConvert.SerializeObject(new List<ArticleListViewModel>());
                ViewBag.HotReviews = JsonConvert.SerializeObject(new List<ArticleListViewModel>());
                ViewBag.RelatedReviews = JsonConvert.SerializeObject(new List<ArticleListViewModel>());
            }
            var relatedNewses = articleService.GetListOfArticles(new GetListOfArticlesParameters
            {
                ArticleType = ArticleType.News,
                Categories = "",
                Tags = article.Tags ?? "",
                OrderString = "",
                Skip = 0,
                Take = 10,
                WhereClauseMore = "",
                SkipArticleIds = article.ArticleId.ToString()
            }).Select(Mapper.Map<ArticleViewModel>);
            ViewBag.RelatedNewses = JsonConvert.SerializeObject(relatedNewses);
            if (Session["LikedCommentIds"] == null)
            {
                Session["LikedCommentIds"] = new List<int>();
            }
            var likedCommentIds = (List<int>)Session["LikedCommentIds"];
            ViewBag.LikedCommentIds = JsonConvert.SerializeObject(likedCommentIds);
            ViewBag.Title = article.Title;
            ViewBag.Description = article.Description;
            ViewBag.Image = article.SquareAvatar.Replace("{width}", "270");
            return View(articleViewModel);
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

        public ActionResult GetPinnedArticles(ArticleType articleType = ArticleType.All, int skip = 0, int take = 20)
        {
            if (Session["Pinned"] == null)
            {
                Session["Pinned"] = new List<ArticleViewModel>();
            }
            return
                Json(((List<ArticleViewModel>)Session["Pinned"]).Where(a => a.ArticleType == articleType || articleType == ArticleType.All).Skip(skip).Take(take), JsonRequestBehavior.AllowGet);
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
                var article = Mapper.Map<ArticleViewModel>(articleService.Queryable().Include(a => a.Opinions).FirstOrDefault(a => a.ArticleId == articleId));
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

        public ActionResult SearchArticles(string keyword, ArticleType articleType, int skip, int take)
        {
            var articles = articleService.SearchArticles(keyword, "", skip, take).Select(Mapper.Map<ArticleViewModel>);
            return Json(articles.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Search(string keyword, int skip = 0, int take = 20)
        {
            var searchResults = articleService.SearchArticles(keyword, "", skip, take).Select(Mapper.Map<ArticleViewModel>).ToList();
            var newses = searchResults.Where(a => a.ArticleType == ArticleType.News);
            ViewBag.Newses = JsonConvert.SerializeObject(newses);
            ViewBag.NewsesUrl = "/Article/SearchArticles?keyword=" + keyword + "&articleType=" + ArticleType.News + "&";
            var reviews = searchResults.Where(a => a.ArticleType == ArticleType.Reviews);
            ViewBag.Reviews = JsonConvert.SerializeObject(reviews);
            ViewBag.ReviewsUrl = "/Article/SearchArticles?keyword=" + keyword + "&articleType=" + ArticleType.Reviews + "&";
            var hotNewses = articleService.GetHotNewses(0, 4, "").Select(Mapper.Map<ArticleViewModel>);
            ViewBag.HotNewses = JsonConvert.SerializeObject(hotNewses);
            var hotReviews = articleService.GetHotReviews(0, 4, "").Select(Mapper.Map<ArticleViewModel>);
            ViewBag.HotReviews = JsonConvert.SerializeObject(hotReviews);
            ViewBag.Keyword = keyword;
            ViewBag.Title = String.Format("Những bài viết chứa từ khóa '{0}'", keyword);
            return View("List");
        }

        public ActionResult GetListOfArticles(string tags = "", string categories = "", ArticleType articleType = ArticleType.All, bool isHot = false, int skip = 0, int take = 20)
        {
            var articles = articleService.GetListOfArticles(new GetListOfArticlesParameters
            {
                ArticleType = articleType,
                Categories = categories,
                Tags = tags,
                OrderString = "",
                SkipArticleIds = "",
                WhereClauseMore = isHot ? " a.IsHot = 1 " : "",
                Skip = skip,
                Take = take
            }).Select(Mapper.Map<ArticleViewModel>);
            return Json(articles.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetList(string tags = "", string categories = "", ArticleType articleType = ArticleType.All, bool isHot = false, int skip = 0, int take = 20)
        {
            var articles = articleService.GetListOfArticles(new GetListOfArticlesParameters
            {
                ArticleType = articleType,
                Categories = categories,
                Tags = tags,
                OrderString = "",
                SkipArticleIds = "",
                WhereClauseMore = isHot ? " a.IsHot = 1 " : "",
                Skip = skip,
                Take = take
            }).Select(Mapper.Map<ArticleViewModel>).ToList();
            var newses = articles.Where(a => a.ArticleType == ArticleType.News);
            ViewBag.Newses = JsonConvert.SerializeObject(newses);
            ViewBag.NewsesUrl = "/Article/GetListOfArticles?tags=" + tags + "&categories=" + categories + "&articleType=" + ArticleType.News + "&isHot=" + isHot + "&";
            var reviews = articles.Where(a => a.ArticleType == ArticleType.Reviews);
            ViewBag.Reviews = JsonConvert.SerializeObject(reviews);
            ViewBag.ReviewsUrl = "/Article/GetListOfArticles?tags=" + tags + "&categories=" + categories + "&articleType=" + ArticleType.Reviews + "&isHot=" + isHot + "&";
            var hotNewses = articleService.GetHotNewses(0, 4, "").Select(Mapper.Map<ArticleViewModel>);
            ViewBag.HotNewses = JsonConvert.SerializeObject(hotNewses);
            var hotReviews = articleService.GetHotReviews(0, 4, "").Select(Mapper.Map<ArticleViewModel>);
            ViewBag.HotReviews = JsonConvert.SerializeObject(hotReviews);
            
            var isHotText = isHot ? " nóng " : " mới ";
            var articleTypeText = "";
            switch (articleType)
            {
                case ArticleType.All:
                    articleTypeText = "";
                    break;
                case ArticleType.News:
                    articleTypeText = "Tin " + isHotText;
                    break;
                case ArticleType.Reviews:
                    articleTypeText = "Soi " + isHotText;
                    break;
            }
            var tagsText = !string.IsNullOrEmpty(tags) ? "Nhãn: '" + tags + "' " : "";
            var categoriesText = !string.IsNullOrEmpty(categories) ? string.Join(", ", articleCategoryService.Queryable().Where(a => categories.Contains(a.SeoName)).Select(a => a.ArticleCategoryName).ToList()) : "";
            ViewBag.Title = String.Format("{0}{1}{2}", articleTypeText, tagsText, categoriesText);
            return View("List");
        }

        public ActionResult GetPinned(int skip = 0, int take = 20)
        {
            if (Session["Pinned"] == null)
            {
                Session["Pinned"] = new List<ArticleViewModel>();
            }
            var articles = (List<ArticleViewModel>)Session["Pinned"];
            var newses = articles.Where(a => a.ArticleType == ArticleType.News);
            ViewBag.Newses = JsonConvert.SerializeObject(newses);
            ViewBag.NewsesUrl = "/Article/GetPinnedArticles?articleType=" + ArticleType.News + "&";
            var reviews = articles.Where(a => a.ArticleType == ArticleType.Reviews);
            ViewBag.Reviews = JsonConvert.SerializeObject(reviews);
            ViewBag.ReviewsUrl = "/Article/GetPinnedArticles?articleType=" + ArticleType.Reviews + "&";
            var hotNewses = articleService.GetHotNewses(0, 4, "").Select(Mapper.Map<ArticleViewModel>);
            ViewBag.HotNewses = JsonConvert.SerializeObject(hotNewses);
            var hotReviews = articleService.GetHotReviews(0, 4, "").Select(Mapper.Map<ArticleViewModel>);
            ViewBag.HotReviews = JsonConvert.SerializeObject(hotReviews);
            ViewBag.Title = "Những bài viết đã ghim";
            return View("List");
        }
    }
}