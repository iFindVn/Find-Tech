using AutoMapper;
using FindTech.Entities.Models;
using FindTech.Services;
using FindTech.Web.Areas.BO.Models;
using Newtonsoft.Json;
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
        public SpecDetailBOController(ISpecDetailService specDetailService, ISpecService specService, IUnitOfWorkAsync unitOfWork)
        {
            this.specDetailService = specDetailService;
            this.specService = specService;
            this.unitOfWork = unitOfWork;
        }

        public ActionResult GetSpecDetails()
        {
            var specDetails = specDetailService.Query().Select();
            return Json(specDetails.Select(Mapper.Map<SpecDetailBOViewModel>), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create(string models)
        {
            var specDetailBOViewModels = JsonConvert.DeserializeObject<List<SpecDetailGridBOViewModel>>(models);
            for (var i = 0; i < specDetailBOViewModels.Count; i++)
            {
                var specDetailBO = specDetailBOViewModels.ElementAt(i);
                var spec = Mapper.Map<SpecDetail>(specDetailBO);
                if (spec.SpecDetailId > 0)
                {
                    specDetailService.Update(spec);
                }
                else
                {
                    specDetailService.Insert(spec);
                }
                
                //specDetailBOViewModels.RemoveAt(i);
                //specDetailBOViewModels.Add(Mapper.Map<SpecDetailGridBOViewModel>(spec));
            }
            unitOfWork.SaveChanges();
            return Json(specDetailBOViewModels, JsonRequestBehavior.AllowGet);
            //return Json(false);
        }

        public ActionResult GetSpecsForGrid(int deviceId)
        {
            var specDetailFromDB = specDetailService.Query().Select().Where(s => s.DeviceId == deviceId);
            var specs = specService.Query().Include(a => a.SpecGroup).Select();


            var query = (from spec in specs
                        join specDetail in specDetailFromDB on spec.SpecId equals specDetail.SpecId into specGrid
                        from subSpec in specGrid.DefaultIfEmpty()
                        select new {spec.SpecId, spec.SpecName, spec.SpecGroupId, spec.SpecGroup.SpecGroupName,
                            DeviceId = (subSpec == null ? deviceId : subSpec.DeviceId),
                            Value = (subSpec == null ? String.Empty: subSpec.Value),
                            SpecDetailId = (subSpec == null ? 0 : subSpec.SpecDetailId),
                            HighLight = (subSpec == null ? false : subSpec.HighLight)
                        }).ToList();
            //var specDetails = specs.Select(Mapper.Map<SpecDetailGridBOViewModel>);

            //if (deviceId > 0)
            //{
            //    specDetails = specDetails.Select(a => { a.DeviceId = deviceId; return a; });
            //}
            
            return Json(query, JsonRequestBehavior.AllowGet);
            //return Json(specs.Select(Mapper.Map<SpecDetailGridBOViewModel>), JsonRequestBehavior.AllowGet);
        }
    }
}