using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using FindTech.Entities.Models;
using FindTech.Services;
using FindTech.Web.Areas.BO.Models;
using Newtonsoft.Json;
using Repository.Pattern.UnitOfWork;

namespace FindTech.Web.Areas.BO.Controllers
{
    public class SourceBOController : Controller
    {
        private ISourceService sourceService { get; set; }
        private IUnitOfWorkAsync unitOfWork { get; set; }

        public SourceBOController(ISourceService sourceService, IUnitOfWorkAsync unitOfWork)
        {
            this.sourceService = sourceService;
            this.unitOfWork = unitOfWork;
        }

        // GET: BO/Source
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult GetSources()
        {
            var sources = sourceService.Query().Select();
            return Json(sources.Select(Mapper.Map<SourceBOViewModel>), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(string models)
        {
            var sourceBOViewModels = JsonConvert.DeserializeObject<List<SourceBOViewModel>>(models);
            for (var i = 0; i < sourceBOViewModels.Count; i++)
            {
                var sourceBOViewModel = sourceBOViewModels.ElementAt(i);
                var source = Mapper.Map<Source>(sourceBOViewModel);
                sourceService.Insert(source);
                unitOfWork.SaveChanges();
                sourceBOViewModels.RemoveAt(i);
                sourceBOViewModels.Add(Mapper.Map<SourceBOViewModel>(source));
            }
            return Json(sourceBOViewModels, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(string models)
        {
            var sourceBOViewModels = JsonConvert.DeserializeObject<List<SourceBOViewModel>>(models);
            for (var i = 0; i < sourceBOViewModels.Count; i++)
            {
                var sourceBOViewModel = sourceBOViewModels.ElementAt(i);
                var source = Mapper.Map<Source>(sourceBOViewModel);
                sourceService.Update(source);
                unitOfWork.SaveChanges();
                sourceBOViewModels.RemoveAt(i);
                sourceBOViewModels.Add(Mapper.Map<SourceBOViewModel>(source));
            }
            return Json(sourceBOViewModels, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Destroy(string models)
        {
            var sourceBOViewModels = JsonConvert.DeserializeObject<List<SourceBOViewModel>>(models);
            for (var i = 0; i < sourceBOViewModels.Count; i++)
            {
                var sourceBOViewModel = sourceBOViewModels.ElementAt(i);
                var source = Mapper.Map<Source>(sourceBOViewModel);
                sourceService.Delete(source);
                unitOfWork.SaveChanges();
                sourceBOViewModels.RemoveAt(i);
            }
            return Json(sourceBOViewModels, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateSource(Source source)
        {
            sourceService.Insert(source);

            unitOfWork.SaveChanges();
            return Redirect("Index");
        }
    }
}