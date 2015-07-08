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
    public class SpecBOController : Controller
    {
        private ISpecService specService { get; set; }
        private IUnitOfWorkAsync unitOfWork { get; set; }

        public SpecBOController(ISpecService specService, IUnitOfWorkAsync unitOfWork)
        {
            this.specService = specService;
            this.unitOfWork = unitOfWork;
        }

        // GET: BO/Spec
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult GetSpecs()
        {
            var specs = specService.Query().Include(a => a.SpecGroup).Select();
            return Json(specs.Select(Mapper.Map<SpecBOViewModel>), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(string models)
        {
            var specBOViewModels = JsonConvert.DeserializeObject<List<SpecBOViewModel>>(models);
            for (var i = 0; i < specBOViewModels.Count; i++)
            {
                var specBOViewModel = specBOViewModels.ElementAt(i);
                var spec = Mapper.Map<Spec>(specBOViewModel);
                spec.SpecGroupId = specBOViewModel.SpecGroup.SpecGroupId;
                specService.Insert(spec);
                
                specBOViewModels.RemoveAt(i);
                specBOViewModels.Add(Mapper.Map<SpecBOViewModel>(spec));
            }
            return Json(specBOViewModels, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(string models)
        {
            var specBOViewModels = JsonConvert.DeserializeObject<List<SpecBOViewModel>>(models);
            for (var i = 0; i < specBOViewModels.Count; i++)
            {
                var specBOViewModel = specBOViewModels.ElementAt(i);
                var spec = Mapper.Map<Spec>(specBOViewModel);
                spec.SpecGroupId = specBOViewModel.SpecGroup.SpecGroupId;
                specService.Update(spec);
                unitOfWork.SaveChanges();
                specBOViewModels.RemoveAt(i);
                specBOViewModels.Add(Mapper.Map<SpecBOViewModel>(spec));
            }
            return Json(specBOViewModels, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Destroy(string models)
        {
            var specBOViewModels = JsonConvert.DeserializeObject<List<SpecBOViewModel>>(models);
            for (var i = 0; i < specBOViewModels.Count; i++)
            {
                var specBOViewModel = specBOViewModels.ElementAt(i);
                var spec = Mapper.Map<Spec>(specBOViewModel);
                spec.SpecGroupId = specBOViewModel.SpecGroup.SpecGroupId;
                specService.Delete(spec);
                unitOfWork.SaveChanges();
                specBOViewModels.RemoveAt(i);
            }
            return Json(specBOViewModels, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateSpec(Spec spec)
        {
            specService.Insert(spec);

            unitOfWork.SaveChanges();
            return Redirect("Index");
        }
    }
}