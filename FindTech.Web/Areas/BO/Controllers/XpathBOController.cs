using System;
using System.Collections.Generic;
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

namespace FindTech.Web.Areas.BO.Controllers
{
    public class XpathBOController : Controller
    {
        private IXpathService xpathService { get; set; }
        private IUnitOfWorkAsync unitOfWork { get; set; }
        // GET: BO/Xpath

        public XpathBOController(IXpathService xpathService, IUnitOfWorkAsync unitOfWork)
        {
            this.xpathService = xpathService;
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

        public ActionResult GetXpaths(int sourceId)
        {
            var xpaths = xpathService.Query(a => a.Source.SourceId == sourceId).Select();
            return Json(xpaths.Select(Mapper.Map<XpathBOViewModel>), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(string models, int sourceId)
        {
            var xpathBOViewModels = JsonConvert.DeserializeObject<List<XpathBOViewModel>>(models);
            for (var i = 0; i < xpathBOViewModels.Count; i++)
            {
                var xpathBOViewModel = xpathBOViewModels.ElementAt(i);
                var xpath = Mapper.Map<Xpath>(xpathBOViewModel);
                xpath.SourceId = sourceId;
                xpathService.Insert(xpath);
                unitOfWork.SaveChanges();
                xpathBOViewModels.RemoveAt(i);
                xpathBOViewModels.Add(Mapper.Map<XpathBOViewModel>(xpath));
            }
            return Json(xpathBOViewModels, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(string models, int sourceId)
        {
            var xpathBOViewModels = JsonConvert.DeserializeObject<List<XpathBOViewModel>>(models);
            for (var i = 0; i < xpathBOViewModels.Count; i++)
            {
                var xpathBOViewModel = xpathBOViewModels.ElementAt(i);
                var xpath = Mapper.Map<Xpath>(xpathBOViewModel);
                xpath.SourceId = sourceId;
                xpathService.Update(xpath);
                unitOfWork.SaveChanges();
                xpathBOViewModels.RemoveAt(i);
                xpathBOViewModels.Add(Mapper.Map<XpathBOViewModel>(xpath));
            }
            return Json(xpathBOViewModels, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Destroy(string models)
        {
            var xpathBOViewModels = JsonConvert.DeserializeObject<List<XpathBOViewModel>>(models);
            for (var i = 0; i < xpathBOViewModels.Count; i++)
            {
                var xpathBOViewModel = xpathBOViewModels.ElementAt(i);
                var xpath = Mapper.Map<Xpath>(xpathBOViewModel);
                xpathService.Delete(xpath);
                unitOfWork.SaveChanges();
                xpathBOViewModels.RemoveAt(i);
            }
            return Json(xpathBOViewModels, JsonRequestBehavior.AllowGet);
        }
    }
}