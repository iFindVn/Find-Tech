using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.XPath;
using AutoMapper;
using FindTech.Entities.Models;
using FindTech.Services;
using FindTech.Web.Areas.BO.Models;
using Newtonsoft.Json;
using Repository.Pattern.UnitOfWork;
using System.Net;

namespace FindTech.Web.Areas.BO.Controllers
{
    public class ContentSectionBOController : Controller
    {
        private IContentSectionService contentSectionService { get; set; }
        private IArticleService articleService { get; set; }
        private IBenchmarkGroupService benchmarkGroupService { get; set; }
        private IImageService imageService { get; set; }
        private IUnitOfWorkAsync unitOfWork { get; set; }
        // GET: BO/ContentSection

        public ContentSectionBOController(IContentSectionService contentSectionService, IArticleService articleService, IImageService imageService, IBenchmarkGroupService benchmarkGroupService, IUnitOfWorkAsync unitOfWork)
        {
            this.contentSectionService = contentSectionService;
            this.articleService = articleService;
            this.benchmarkGroupService = benchmarkGroupService;
            this.imageService = imageService;
            this.unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            return View();
        }

     
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult GetContentSections(int articleId)
        {
            var contentSections = contentSectionService.Query(a => a.Article.ArticleId == articleId).Select();
            return Json(contentSections.Select(Mapper.Map<ContentSectionBOViewModel>), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(ContentSectionBOViewModel contentSectionBOViewModel)
        {
            var contentSection = Mapper.Map<ContentSection>(contentSectionBOViewModel);

            contentSection.SectionContent = WebUtility.HtmlDecode(contentSection.SectionContent);
            contentSection.SectionDescription = WebUtility.HtmlDecode(contentSection.SectionDescription);

            if (contentSection.ContentSectionId != 0)
            {
                var count =
                    contentSectionService.Queryable().Count(a => a.ContentSectionId == contentSection.ContentSectionId);
                if (count > 0)
                {
                    contentSectionService.Update(contentSection);
                }
            }
            else
            {
                contentSection.Article = articleService.Find(contentSection.ArticleId);
                contentSectionService.Insert(contentSection);
            }
            unitOfWork.SaveChanges();
            contentSection.Images = contentSectionService.Queryable().Include(a => a.Images).FirstOrDefault(a => a.ContentSectionId == contentSection.ContentSectionId).Images;
            return View("_ContentSectionForm", Mapper.Map<ContentSectionBOViewModel>(contentSection));
        }

        //[HttpPost]
        //public ActionResult Create(string models, int articleId)
        //{
        //    var contentSectionBOViewModels = JsonConvert.DeserializeObject<List<ContentSectionBOViewModel>>(models);
        //    for (var i = 0; i < contentSectionBOViewModels.Count; i++)
        //    {
        //        var contentSectionBOViewModel = contentSectionBOViewModels.ElementAt(i);
        //        var contentSection = Mapper.Map<ContentSection>(contentSectionBOViewModel);
        //        contentSection.ArticleId = articleId;
        //        contentSectionService.Insert(contentSection);
        //        unitOfWork.SaveChanges();
        //        contentSectionBOViewModels.RemoveAt(i);
        //        contentSectionBOViewModels.Add(Mapper.Map<ContentSectionBOViewModel>(contentSection));
        //    }
        //    return Json(contentSectionBOViewModels, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public ActionResult Update(string models, int articleId)
        {
            var contentSectionBOViewModels = JsonConvert.DeserializeObject<List<ContentSectionBOViewModel>>(models);
            for (var i = 0; i < contentSectionBOViewModels.Count; i++)
            {
                var contentSectionBOViewModel = contentSectionBOViewModels.ElementAt(i);

                contentSectionBOViewModel.SectionContent = WebUtility.HtmlDecode(contentSectionBOViewModel.SectionContent);
                contentSectionBOViewModel.SectionDescription = WebUtility.HtmlDecode(contentSectionBOViewModel.SectionDescription);

                var contentSection = Mapper.Map<ContentSection>(contentSectionBOViewModel);
                contentSection.ArticleId = articleId;
                contentSectionService.Update(contentSection);
                unitOfWork.SaveChanges();
                contentSectionBOViewModels.RemoveAt(i);
                contentSectionBOViewModels.Add(Mapper.Map<ContentSectionBOViewModel>(contentSection));
            }
            return Json(contentSectionBOViewModels, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Destroy(int contentSectionId)
        {
            var contentSection =
                contentSectionService.Queryable().Include(a => a.Images).FirstOrDefault();
            if (contentSection != null)
            {
                foreach (var image in contentSection.Images)
                {
                    imageService.Delete(image);
                }
                contentSectionService.Delete(contentSection);
                unitOfWork.SaveChanges();
            }
            return Json(contentSectionId, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _ContentSectionForm(int? contentSectionId, int articleId)
        {
            var contentSection = new ContentSectionBOViewModel { ArticleId = articleId };
            if (contentSectionId != null)
            {
                contentSection = Mapper.Map<ContentSectionBOViewModel>(contentSectionService.Queryable().Include(a => a.Images).FirstOrDefault(a => a.ContentSectionId == contentSectionId));
            }
            return View(contentSection);
        }

        [HttpPost]
        public ActionResult AddImage(Image image, int contentSectionId)
        {
            var contentSection =
                contentSectionService.Queryable().FirstOrDefault(a => a.ContentSectionId == contentSectionId);
            if (contentSection != null)
            {
                imageService.Insert(image);
                contentSection.Images.Add(image);
                contentSectionService.Update(contentSection);
                unitOfWork.SaveChanges();
            }
            return Json(image.ImageId, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteImage(int imageId, int contentSectionId)
        {
            var image = imageService.Queryable().FirstOrDefault(a => a.ImageId == imageId);
            var contentSection =
                contentSectionService.Queryable().FirstOrDefault(a => a.ContentSectionId == contentSectionId);
            if (contentSection != null && image != null)
            {
                imageService.Delete(image);
                contentSection.Images.Remove(image);
                contentSectionService.Update(contentSection);
                unitOfWork.SaveChanges();
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}