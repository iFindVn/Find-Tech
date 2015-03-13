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
    public class SpecGroupBOController : Controller
    {
        private ISpecGroupService specGroupService { get; set; }
        private IUnitOfWorkAsync unitOfWork { get; set; }

        public SpecGroupBOController(ISpecGroupService specGroupService, IUnitOfWorkAsync unitOfWork)
        {
            this.specGroupService = specGroupService;
            this.unitOfWork = unitOfWork;
        }

        // GET: BO/SpecGroup
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult GetSpecGroups()
        {
            var specGroups = specGroupService.Query().Select();
            return Json(specGroups.Select(Mapper.Map<SpecGroupBOViewModel>), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(string models)
        {
            var specGroupBOViewModels = JsonConvert.DeserializeObject<List<SpecGroupBOViewModel>>(models);
            for (var i = 0; i < specGroupBOViewModels.Count; i++)
            {
                var specGroupBOViewModel = specGroupBOViewModels.ElementAt(i);
                var specGroup = Mapper.Map<SpecGroup>(specGroupBOViewModel);
                specGroupService.Insert(specGroup);
                unitOfWork.SaveChanges();
                specGroupBOViewModels.RemoveAt(i);
                specGroupBOViewModels.Add(Mapper.Map<SpecGroupBOViewModel>(specGroup));
            }
            return Json(specGroupBOViewModels, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(string models)
        {
            var specGroupBOViewModels = JsonConvert.DeserializeObject<List<SpecGroupBOViewModel>>(models);
            for (var i = 0; i < specGroupBOViewModels.Count; i++)
            {
                var specGroupBOViewModel = specGroupBOViewModels.ElementAt(i);
                var specGroup = Mapper.Map<SpecGroup>(specGroupBOViewModel);
                specGroupService.Update(specGroup);
                unitOfWork.SaveChanges();
                specGroupBOViewModels.RemoveAt(i);
                specGroupBOViewModels.Add(Mapper.Map<SpecGroupBOViewModel>(specGroup));
            }
            return Json(specGroupBOViewModels, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Destroy(string models)
        {
            var specGroupBOViewModels = JsonConvert.DeserializeObject<List<SpecGroupBOViewModel>>(models);
            for (var i = 0; i < specGroupBOViewModels.Count; i++)
            {
                var specGroupBOViewModel = specGroupBOViewModels.ElementAt(i);
                var specGroup = Mapper.Map<SpecGroup>(specGroupBOViewModel);
                specGroupService.Delete(specGroup);
                unitOfWork.SaveChanges();
                specGroupBOViewModels.RemoveAt(i);
            }
            return Json(specGroupBOViewModels, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateSpecGroup(SpecGroup specGroup)
        {
            specGroupService.Insert(specGroup);

            unitOfWork.SaveChanges();
            return Redirect("Index");
        }
    }
}