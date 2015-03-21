using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using System.Xml;
using AutoMapper;
using FindTech.Entities.Models;
using FindTech.Entities.Models.Enums;
using FindTech.Services;
using FindTech.Web.Areas.BO.CommonFunction;
using FindTech.Web.Areas.BO.Models;
using FindTech.Web.Models;
using HtmlAgilityPack;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using Repository.Pattern.UnitOfWork;
using TestImageCrop;
using System.IO;
using System.Configuration;
using System.Net;
using System.Drawing;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace FindTech.Web.Areas.BO.Controllers
{
    [Authorize]
    public class ArticleBOController : BaseController
    {
        private ISourceService sourceService { get; set; }
        private IArticleCategoryService articleCategoryService { get; set; }
        private IArticleService articleService { get; set; }
        private IOpinionService opinionService { get; set; }
        private IUnitOfWorkAsync unitOfWork { get; set; }

        private Cloudinary cloudinary;

        public ArticleBOController(IUnitOfWorkAsync unitOfWork, ISourceService sourceService,
            IArticleService articleService, IArticleCategoryService articleCategoryService, IOpinionService opinionService)
        {
            this.sourceService = sourceService;
            this.articleService = articleService;
            this.articleCategoryService = articleCategoryService;
            this.opinionService = opinionService;
            this.unitOfWork = unitOfWork;

            Account account = new Account(
              "ifind-vn",
              "527894251963919",
              "JxLoCiFSj4Rh5Wvfyek9JzUPetg");

            cloudinary = new Cloudinary(account);
        }

        // GET: BO/Article
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(int? articleId)
        {
            var articleBOViewModel = new ArticleBOViewModel();
            var contentSections = new List<ContentSectionBOViewModel>();
            if (articleId != null)
            {
                var article = articleService.Queryable().Include(a => a.ContentSections).FirstOrDefault(a =>a.ArticleId == articleId);
                articleBOViewModel = Mapper.Map<ArticleBOViewModel>(article);
                if (article != null && article.ContentSections != null)
                    contentSections = article.ContentSections.Select(Mapper.Map<ContentSectionBOViewModel>).ToList();
            }
            ViewBag.ContentSections = contentSections;
            return View(articleBOViewModel);
        }

        public ActionResult GetTags(string tag)
        {
            var tags = new List<object>()
            {
                new {name = "Troy"}
            };
            return Json(tags.Where(a => a.GetType().GetProperty("name").GetValue(a).ToString().Contains(tag)),
                JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetArticles(int skip, int take, String filter)
        {

            var total = articleService.Query().Select().Count(a => a.IsDeleted != true);
            var articles = new List<Article>();
            
            if (filter != null)
            {
                var articleGridListFiltersBOViewModel = JsonConvert.DeserializeObject<ArticleGridListFiltersBOViewModel>(filter);
                var listParame = new List<string>();
                var query = BuildingWhereClause(articleGridListFiltersBOViewModel, listParame);

          
                var whereClause =  new SqlParameter
                {
                    ParameterName = "whereClause",
                    Value = query.ToString(),
                    
                };

                var paramSkip = new SqlParameter
                {
                    ParameterName = "skip",
                    Value = skip.ToString(CultureInfo.InvariantCulture)
                };

                var paramTake = new SqlParameter
                {
                    ParameterName = "take",
                    Value = take.ToString(CultureInfo.InvariantCulture)
                };

                articles = articleService.SelectQuery("exec ifadmin.SP_Article_BO_getArticlesByFiltersPaging @whereClause, @skip, @take", whereClause, paramSkip, paramTake).ToList();
            }
            else
            {  
               articles  = articleService.Queryable().Where(a => a.IsDeleted != true).OrderByDescending(a => a.CreatedDate).Skip(skip).Take(take).ToList();
            }            
            return Json(new { articles = articles.Select(Mapper.Map<ArticleGridBOViewModel>) , totalCount = total}, JsonRequestBehavior.AllowGet);
        }

        private StringBuilder BuildingWhereClause(ArticleGridListFiltersBOViewModel articleGridListFiltersBOViewModel, List<String> Params )
        {
            var query = new StringBuilder();
            query.Append(" ( ");
         
            for (int i = 0; i < articleGridListFiltersBOViewModel.Filters.Count; i++)
            {
                var filter = articleGridListFiltersBOViewModel.Filters[i];
                if (i > 0)
                {
                    query.Append(" ");
                    query.Append(articleGridListFiltersBOViewModel.Logic);
                    query.Append(" ");
                }
                query.Append(filter.Field);
                query.Append(" ");
                if (filter.Operator.Equals("eq"))
                {
                    query.Append(" = '" + filter.Value + "'");
                }
                else if (filter.Operator.Equals("ne"))
                {
                    query.Append(" <>'" + filter.Value + "'" );
                }
                else if (filter.Operator.Equals("contains"))
                {
                    query.Append("Like N'%" + filter.Value + "%' ");
                }
                else if (filter.Operator.Equals("startswith"))
                {
                    query.Append("Like N'%" + filter.Value + "'");
                }
                else if (filter.Operator.Equals("endswith"))
                {
                    query.Append("Like N'" + filter.Value + "%' ");
                }
            }
           
            query.Append(" ) ");
            return query;
        }

        [ValidateInput(false)]
        public ActionResult Destroy(string models)
        {
            var articleBOViewModel = JsonConvert.DeserializeObject<ArticleGridBOViewModel>(models);
            var article = Mapper.Map<Article>(articleBOViewModel);
            article.IsDeleted = true;
            articleService.Update(article);
            var result = unitOfWork.SaveChanges();
            return Json(result);
        }

        [ValidateInput(false)]
        public ActionResult Update(string models)
        {
            var articleBOViewModel = JsonConvert.DeserializeObject<ArticleGridBOViewModel>(models);
            var article = Mapper.Map<Article>(articleBOViewModel);
            article.CreatedUserId = CurrentUser.Id;
            article.CreatedUserDisplayName = CurrentUser.DisplayName;
            articleService.Update(article);
            var result = unitOfWork.SaveChanges();
            return Json(result);
        }



        [HttpPost]
        public ActionResult ReadRss()
        {
            var errorArticles = new List<object>();
            if (!articleCategoryService.Queryable().Any(a => a.ArticleCategoryName == "Tin tổng hợp"))
            {
                articleCategoryService.Insert(new ArticleCategory
                {
                    ArticleCategoryName = "Tin tổng hợp",
                    Color = "info",
                    IsActived = true,
                    Priority = 1,
                    SeoName = "Tin tổng hợp".GenerateSeoTitle()
                });
                unitOfWork.SaveChanges();
            }
            var category =
                articleCategoryService.Queryable().FirstOrDefault(a => a.ArticleCategoryName == "Tin tổng hợp");
            var rssSources = sourceService.Queryable().Include(a => a.Xpaths).ToList();
            foreach (var rssSource in rssSources)
            {
                var xmlReader = XmlReader.Create(rssSource.Link);
                var feed = SyndicationFeed.Load(xmlReader);
                xmlReader.Close();
                if (feed != null)
                {
                    foreach (var feedItem in feed.Items)
                    {
                        var title = feedItem.Title.Text;
                        if (!articleService.Queryable().Any(a => a.Title == title))
                        {
                            var summary = new HtmlAgilityPack.HtmlDocument();
                            summary.LoadHtml(feedItem.Summary != null ? feedItem.Summary.Text : "");
                            var contentDocument = new HtmlAgilityPack.HtmlDocument();
                            var link = feedItem.Links.FirstOrDefault();
                            if (link != null)
                            {
                                var absoluteUri = link.GetAbsoluteUri();
                                if (absoluteUri != null) contentDocument = new HtmlWeb().Load(absoluteUri.AbsoluteUri);
                            }
                            var contentXpaths = rssSource.Xpaths.Where(a => a.ArticleField == ArticleField.Content);
                            var content = "";
                            foreach (
                                var contentNode in
                                    contentXpaths.Select(
                                        contentXpath =>
                                            contentDocument.DocumentNode.SelectSingleNode(contentXpath.XpathString))
                                        .Where(contentNode => contentNode != null))
                            {
                                content = contentNode.InnerHtml;
                                break;
                            }
                            if (category != null)
                            {
                                try
                                {
                                    var seoTitle = title.GenerateSeoTitle();
                                    var avatar = "";
                                    var imageUrl = summary.DocumentNode.Descendants("img")
                                                .Select(n => n.Attributes["src"].Value)
                                                .ToArray()
                                                .FirstOrDefault();
                                    if (imageUrl != null)
                                    {
                                        avatar = imageUrl;
                                        //HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(imageUrl);
                                        //// Sends the HttpWebRequest and waits for the response.			
                                        //HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                                        //// Gets the stream associated with the response.
                                        //Stream receiveStream = myHttpWebResponse.GetResponseStream();

                                        //var imageUploadParams = new ImageUploadParams()
                                        //{
                                        //    File = new FileDescription(seoTitle + "-avatar", receiveStream),
                                        //    PublicId = Path.GetFileNameWithoutExtension(seoTitle + "-avatar")
                                        //};
                                        //var result = cloudinary.Upload(imageUploadParams);
                                        //avatar = result.Uri.ToString();
                                        //avatar = Url.Action("Image", "ImageBO", new { path = seoTitle + "-avatar" });
                                        //return new FileStreamResult(receiveStream, "image/jpg");
                                    }
                                    var article = new Article
                                    {
                                        ArticleCategoryId = category.ArticleCategoryId,
                                        ArticleCategory = category,
                                        Title = title,
                                        Avatar = avatar,
                                        Description = summary.DocumentNode.InnerText,
                                        IsActived = false,
                                        Priority = 1,
                                        PublishedDate = feedItem.PublishDate.DateTime,
                                        BoxSize = BoxSize.Box1,
                                        SourceId = rssSource.SourceId,
                                        Source = rssSource,
                                        Content = content,
                                        ArticleType = ArticleType.News,
                                        SeoTitle = seoTitle,
                                        IsHot = false,
                                        CreatedUserId = CurrentUser.Id,
                                        CreatedUserDisplayName = CurrentUser.DisplayName
                                    };
                                    articleService.Insert(article);
                                    unitOfWork.SaveChanges();
                                    CreateDefaultOpinions(article.ArticleId);
                                }
                                catch(Exception e)
                                {
                                    errorArticles.Add(new { Title = title, PublishedDate = feedItem.PublishDate.DateTime });
                                }
                            }
                        }
                    }
                }
            }

            return Json(new { Success = errorArticles.Count == 0, ErrorArticles = errorArticles});
        }



        [HttpPost]
        public ActionResult CreateOrUpdate(ArticleBOViewModel articleBOViewModel)
        {
            articleBOViewModel.SeoTitle = articleBOViewModel.Title.GenerateSeoTitle();
            var articleId = 0;
            if (articleBOViewModel.ArticleId != 0)
            {
                var count = articleService.Queryable().Count(a => a.ArticleId == articleBOViewModel.ArticleId);
                if (count > 0)
                {
                    var existedArticle = Mapper.Map<Article>(articleBOViewModel);
                    existedArticle.CreatedUserId = CurrentUser.Id;
                    existedArticle.CreatedUserDisplayName = CurrentUser.DisplayName;

                    existedArticle.Content = WebUtility.HtmlDecode(existedArticle.Content);
                    existedArticle.Description = WebUtility.HtmlDecode(existedArticle.Description);

                    articleService.Update(existedArticle);
                    unitOfWork.SaveChanges();
                    articleId = existedArticle.ArticleId;
                    CreateDefaultOpinions(articleId);
                }
            }
            else
            {
                var newArticle = Mapper.Map<Article>(articleBOViewModel);
                newArticle.CreatedUserId = CurrentUser.Id;
                newArticle.CreatedUserDisplayName = CurrentUser.DisplayName;

                newArticle.Content = WebUtility.HtmlDecode(newArticle.Content);
                newArticle.Description = WebUtility.HtmlDecode(newArticle.Description);

                articleService.Insert(newArticle);
                unitOfWork.SaveChanges();
                articleId = newArticle.ArticleId;
            }
            var url = Url.Action("Create", "ArticleBO", new { articleId }, Request.Url.Scheme);
            return Json(url, JsonRequestBehavior.AllowGet);
        }

        public void CreateDefaultOpinions(int articleId)
        {
            var opinionLevels = new List<OpinionLevel>{ OpinionLevel.Excellent, OpinionLevel.Good, OpinionLevel.Average, OpinionLevel.Bad};
            foreach (var opinionLevel in opinionLevels)
            {
                var opinion = opinionService.GetOpinion(articleId, opinionLevel);
                if (opinion == null)
                {
                    opinionService.Insert(new Opinion { OpinionCount = 0, OpinionLevel = opinionLevel, ArticleId = articleId });
                }
            }
        }
       
        [HttpPost]
        public ActionResult ActiveArticle(string articleIds)
        {
            var activeArticleIds = articleIds.Split(',');
            foreach (var activeArticleId in activeArticleIds.Select(a => int.Parse(a)))
            {
                var article = articleService.Queryable().FirstOrDefault(a => a.ArticleId == activeArticleId);
                article.IsActived = true;
                articleService.Update(article);
            }
            unitOfWork.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult CropImage(string imagePath, float scale, int w, int h, int x, int y)
        {
            Rectangle cropRect = new Rectangle((int)(x / scale), (int)(y / scale), (int)(w / scale), (int)(h / scale));
            Bitmap source = System.Drawing.Image.FromFile(imagePath) as Bitmap;
            Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);
            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(source, new Rectangle(0, 0, cropRect.Width, cropRect.Height), cropRect, GraphicsUnit.Pixel);
            }
            return Json(true);
        }
    }
}