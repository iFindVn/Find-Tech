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
    public class BrandBOController : Controller
    {
        private IBrandService brandService { get; set; }
        private IUnitOfWorkAsync unitOfWork { get; set; }

        public BrandBOController(IBrandService brandService, IUnitOfWorkAsync unitOfWork)
        {
            this.brandService = brandService;
            this.unitOfWork = unitOfWork;
        }

        // GET: BO/Brand
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult GetBrands()
        {
            var brands = brandService.Query().Select();
            return Json(brands.Select(Mapper.Map<BrandBOViewModel>), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(string models)
        {
            var brandBOViewModels = JsonConvert.DeserializeObject<List<BrandBOViewModel>>(models);
            for (var i = 0; i < brandBOViewModels.Count; i++)
            {
                var brandBOViewModel = brandBOViewModels.ElementAt(i);
                var brand = Mapper.Map<Brand>(brandBOViewModel);
                brandService.Insert(brand);
                unitOfWork.SaveChanges();
                brandBOViewModels.RemoveAt(i);
                brandBOViewModels.Add(Mapper.Map<BrandBOViewModel>(brand));
            }
            return Json(brandBOViewModels, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(string models)
        {
            var brandBOViewModels = JsonConvert.DeserializeObject<List<BrandBOViewModel>>(models);
            for (var i = 0; i < brandBOViewModels.Count; i++)
            {
                var brandBOViewModel = brandBOViewModels.ElementAt(i);
                var brand = Mapper.Map<Brand>(brandBOViewModel);
                brandService.Update(brand);
                unitOfWork.SaveChanges();
                brandBOViewModels.RemoveAt(i);
                brandBOViewModels.Add(Mapper.Map<BrandBOViewModel>(brand));
            }
            return Json(brandBOViewModels, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Destroy(string models)
        {
            var brandBOViewModels = JsonConvert.DeserializeObject<List<BrandBOViewModel>>(models);
            for (var i = 0; i < brandBOViewModels.Count; i++)
            {
                var brandBOViewModel = brandBOViewModels.ElementAt(i);
                var brand = Mapper.Map<Brand>(brandBOViewModel);
                brandService.Delete(brand);
                unitOfWork.SaveChanges();
                brandBOViewModels.RemoveAt(i);
            }
            return Json(brandBOViewModels, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateBrand(Brand brand)
        {
            brandService.Insert(brand);

            unitOfWork.SaveChanges();
            return Redirect("Index");
        }
    }
}