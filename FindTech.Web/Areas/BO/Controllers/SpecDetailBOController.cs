using AutoMapper;
using FindTech.Services;
using FindTech.Web.Areas.BO.Models;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FindTech.Web.Areas.BO.Controllers
{
    public class SpecDetailBOController: Controller
    {
        private ISpecDetailService specDetailService { get; set; }
        private ISpecService specService { get; set; }
        private IUnitOfWorkAsync unitOfWork { get; set; }
        public SpecDetailBOController(ISpecDetailService specDetailService, IUnitOfWorkAsync unitOfWork)
        {
            this.specDetailService = specDetailService;
            this.unitOfWork = unitOfWork;
        }

        public ActionResult GetSpecDetails()
        {
            var specDetails = specDetailService.Query().Select();
            return Json(specDetails.Select(Mapper.Map<SpecDetailBOViewModel>), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create(string models)
        {
            return Json(false);
        }

        public ActionResult GetSpecsForGrid()
        {
            var specs = specService.Query().Include(a => a.SpecGroup).Select();
            return Json(specs.Select(Mapper.Map<SpecDetailGridBOViewModel>), JsonRequestBehavior.AllowGet);
        }
    }
}