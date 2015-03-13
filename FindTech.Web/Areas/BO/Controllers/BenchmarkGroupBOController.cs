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
    public class BenchmarkGroupBOController : Controller
    {
        private IBenchmarkGroupService benchmarkGroupService { get; set; }
        private IUnitOfWorkAsync unitOfWork { get; set; }

        public BenchmarkGroupBOController(IBenchmarkGroupService benchmarkGroupService, IUnitOfWorkAsync unitOfWork)
        {
            this.benchmarkGroupService = benchmarkGroupService;
            this.unitOfWork = unitOfWork;
        }

        // GET: BO/BenchmarkGroup
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult GetBenchmarkGroups()
        {
            var benchmarkGroups = benchmarkGroupService.Query().Include(a => a.Parent).Select();
            var benchmarkGroupViewModels = benchmarkGroups.Select(Mapper.Map<BenchmarkGroupBOViewModel>).ToList();
            return Json(benchmarkGroupViewModels, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(string models)
        {
            var benchmarkGroupBOViewModels = JsonConvert.DeserializeObject<List<BenchmarkGroupBOViewModel>>(models);
            for (var i = 0; i < benchmarkGroupBOViewModels.Count; i++)
            {
                var benchmarkGroupBOViewModel = benchmarkGroupBOViewModels.ElementAt(i);
                var benchmarkGroup = Mapper.Map<BenchmarkGroup>(benchmarkGroupBOViewModel);
                if (benchmarkGroup.Parent.BenchmarkGroupId == 0)
                {
                    benchmarkGroup.Parent = null;
                }
                else
                {
                    benchmarkGroup.ParentId = benchmarkGroupBOViewModel.Parent.BenchmarkGroupId;
                    benchmarkGroup.Parent = benchmarkGroupService.Queryable()
                        .FirstOrDefault(a => a.BenchmarkGroupId == benchmarkGroup.ParentId);
                }
                benchmarkGroupService.Insert(benchmarkGroup);
                unitOfWork.SaveChanges();
                benchmarkGroupBOViewModels.RemoveAt(i);
                benchmarkGroupBOViewModels.Add(Mapper.Map<BenchmarkGroupBOViewModel>(benchmarkGroup));
            }
            return Json(benchmarkGroupBOViewModels, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(string models)
        {
            var benchmarkGroupBOViewModels = JsonConvert.DeserializeObject<List<BenchmarkGroupBOViewModel>>(models);
            for (var i = 0; i < benchmarkGroupBOViewModels.Count; i++)
            {
                var benchmarkGroupBOViewModel = benchmarkGroupBOViewModels.ElementAt(i);
                var benchmarkGroup = Mapper.Map<BenchmarkGroup>(benchmarkGroupBOViewModel);
                if (benchmarkGroup.Parent.BenchmarkGroupId == 0)
                {
                    benchmarkGroup.Parent = null;
                }
                else
                {
                    benchmarkGroup.ParentId = benchmarkGroupBOViewModel.Parent.BenchmarkGroupId;
                    benchmarkGroup.Parent =
                        benchmarkGroupService.Queryable()
                            .FirstOrDefault(a => a.BenchmarkGroupId == benchmarkGroup.ParentId);
                }
                benchmarkGroupService.Update(benchmarkGroup);
                unitOfWork.SaveChanges();
                benchmarkGroupBOViewModels.RemoveAt(i);
                benchmarkGroupBOViewModels.Add(Mapper.Map<BenchmarkGroupBOViewModel>(benchmarkGroup));
            }
            return Json(benchmarkGroupBOViewModels, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Destroy(string models)
        {
            var benchmarkGroupBOViewModels = JsonConvert.DeserializeObject<List<BenchmarkGroupBOViewModel>>(models);
            for (var i = 0; i < benchmarkGroupBOViewModels.Count; i++)
            {
                var benchmarkGroupBOViewModel = benchmarkGroupBOViewModels.ElementAt(i);
                var benchmarkGroup =
                    benchmarkGroupService.Queryable()
                        .FirstOrDefault(a => a.BenchmarkGroupId == benchmarkGroupBOViewModel.BenchmarkGroupId);
                benchmarkGroupService.Delete(benchmarkGroup);
                unitOfWork.SaveChanges();
                benchmarkGroupBOViewModels.RemoveAt(i);
            }
            return Json(benchmarkGroupBOViewModels, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateBenchmarkGroup(BenchmarkGroup benchmarkGroup)
        {
            benchmarkGroupService.Insert(benchmarkGroup);

            unitOfWork.SaveChanges();
            return Redirect("Index");
        }
    }
}