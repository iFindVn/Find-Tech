using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using FindTech.Entities.Models;
using FindTech.Services;
using FindTech.Web.Areas.BO.CommonFunction;
using FindTech.Web.Areas.BO.Models;
using Newtonsoft.Json;
using Repository.Pattern.UnitOfWork;

namespace FindTech.Web.Areas.BO.Controllers
{
    public class ArticleCategoryBOController : Controller
    {
        private IArticleCategoryService articleCategoryService { get; set; }
        private IUnitOfWorkAsync unitOfWork { get; set; }

        public ArticleCategoryBOController(IArticleCategoryService articleCategoryService,IUnitOfWorkAsync unitOfWork)
        {
            this.articleCategoryService = articleCategoryService;
            this.unitOfWork = unitOfWork;
        }

        // GET: BO/ArticleCategory
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult GetArticleCategories()
        {
            var articleCategories = articleCategoryService.Query(a => a.IsDeleted != true).Select();
            return Json(articleCategories.Select(Mapper.Map<ArticleCategoryBOViewModel>), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(string models)
        {
            var articleCategoryBOViewModels = JsonConvert.DeserializeObject<List<ArticleCategoryBOViewModel>>(models);
            for (var i = 0; i < articleCategoryBOViewModels.Count; i++)
            {
                var articleCategoryBOViewModel = articleCategoryBOViewModels.ElementAt(i);
                var articleCategory = Mapper.Map<ArticleCategory>(articleCategoryBOViewModel);
                articleCategory.SeoName = articleCategory.ArticleCategoryName.GenerateSeoTitle();
                articleCategoryService.Insert(articleCategory);
                unitOfWork.SaveChanges();
                articleCategoryBOViewModels.RemoveAt(i);
                articleCategoryBOViewModels.Add(Mapper.Map<ArticleCategoryBOViewModel>(articleCategory));
            }
            return Json(articleCategoryBOViewModels, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(string models)
        {
            var articleCategoryBOViewModels = JsonConvert.DeserializeObject<List<ArticleCategoryBOViewModel>>(models);
            for (var i = 0; i < articleCategoryBOViewModels.Count; i++)
            {
                var articleCategoryBOViewModel = articleCategoryBOViewModels.ElementAt(i);
                var articleCategory = Mapper.Map<ArticleCategory>(articleCategoryBOViewModel);
                articleCategory.SeoName = articleCategory.ArticleCategoryName.GenerateSeoTitle();
                articleCategoryService.Update(articleCategory);
                unitOfWork.SaveChanges();
                articleCategoryBOViewModels.RemoveAt(i);
                articleCategoryBOViewModels.Add(Mapper.Map<ArticleCategoryBOViewModel>(articleCategory));
            }
            return Json(articleCategoryBOViewModels, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Destroy(string models)
        {
            var articleCategoryBOViewModels = JsonConvert.DeserializeObject<List<ArticleCategoryBOViewModel>>(models);
            for (var i = 0; i < articleCategoryBOViewModels.Count; i++)
            {
                var articleCategoryBOViewModel = articleCategoryBOViewModels.ElementAt(i);
                var articleCategory = Mapper.Map<ArticleCategory>(articleCategoryBOViewModel);
                articleCategory.IsDeleted = true;
                articleCategoryService.Update(articleCategory);
                unitOfWork.SaveChanges();
                articleCategoryBOViewModels.RemoveAt(i);
                articleCategoryBOViewModels.Add(Mapper.Map<ArticleCategoryBOViewModel>(articleCategory));
            }
            return Json(articleCategoryBOViewModels, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateArticleCategory(ArticleCategory articleCategory)
        {
            articleCategoryService.Insert(articleCategory);

            unitOfWork.SaveChanges();
            return Redirect("Index");
        }
    }
}