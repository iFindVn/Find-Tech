using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using AutoMapper;
using FindTech.Entities.Models;
using FindTech.Web.Areas.BO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FindTech.Services;
using Newtonsoft.Json;
using Repository.Pattern.UnitOfWork;

namespace FindTech.Web.Areas.BO.Controllers
{
    public class DeviceBOController : Controller
    {
        private IDeviceService deviceService { get; set; }
        private ISpecService specService { get; set; }
        private IUnitOfWorkAsync unitOfWork { get; set; }

        public DeviceBOController(IDeviceService deviceService, ISpecService specService, IUnitOfWorkAsync unitOfWork)
        {
            this.deviceService = deviceService;
            this.unitOfWork = unitOfWork;
            this.specService = specService;
        }
        // GET: BO/DevicesBO
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetDevices(int skip, int take, String filter)
        {

            var total = deviceService.Query().Select().Count(a => a.IsDeleted != true);
            var devices = new List<Device>();

            if (filter != null)
            {
                var deviceGridFilterBOViewModel = JsonConvert.DeserializeObject<DeviceGridListFiltersBOViewModel>(filter);
                var listParame = new List<string>();
                var query = BuildingWhereClause(deviceGridFilterBOViewModel, listParame);


                var whereClause = new SqlParameter
                {
                    ParameterName = "whereClause",
                    Value = query.ToString(),

                };

                var paramSkip = new SqlParameter
                {
                    ParameterName = "skip",
                    Value = skip.ToString(CultureInfo.InvariantCulture)
                };

                var paramTake = new SqlParameter
                {
                    ParameterName = "take",
                    Value = take.ToString(CultureInfo.InvariantCulture)
                };

                devices = deviceService.SelectQuery("exec ifadmin.SP_Article_BO_getDevicesByFiltersPaging @whereClause, @skip, @take", whereClause, paramSkip, paramTake).ToList();
            }
            else
            {
                devices = deviceService.Queryable().Where(a => a.IsDeleted != true).OrderByDescending(a => a.CreatedDate).Skip(skip).Take(take).ToList();
            }
            return Json(new { devices = devices.Select(Mapper.Map<DeviceGridBOViewModel>), totalCount = total }, JsonRequestBehavior.AllowGet);
        }

        private StringBuilder BuildingWhereClause(DeviceGridListFiltersBOViewModel deviceGridListFiltersBOViewModel, List<String> Params)
        {
            var query = new StringBuilder();
            query.Append(" ( ");

            for (int i = 0; i < deviceGridListFiltersBOViewModel.Filters.Count; i++)
            {
                var filter = deviceGridListFiltersBOViewModel.Filters[i];
                if (i > 0)
                {
                    query.Append(" ");
                    query.Append(deviceGridListFiltersBOViewModel.Logic);
                    query.Append(" ");
                }
                query.Append(filter.Field);
                query.Append(" ");
                if (filter.Operator.Equals("eq"))
                {
                    query.Append(" = '" + filter.Value + "'");
                }
                else if (filter.Operator.Equals("ne"))
                {
                    query.Append(" <>'" + filter.Value + "'");
                }
                else if (filter.Operator.Equals("contains"))
                {
                    query.Append("Like N'%" + filter.Value + "%' ");
                }
                else if (filter.Operator.Equals("startswith"))
                {
                    query.Append("Like N'%" + filter.Value + "'");
                }
                else if (filter.Operator.Equals("endswith"))
                {
                    query.Append("Like N'" + filter.Value + "%' ");
                }
            }

            query.Append(" ) ");
            return query;
        }

        [ValidateInput(false)]
        public ActionResult Destroy(string models)
        {
            var deviceBOViewModel = JsonConvert.DeserializeObject<DeviceGridBOViewModel>(models);
            var device = Mapper.Map<Device>(deviceBOViewModel);
            device.IsDeleted = true;
            deviceService.Update(device);
            var result = unitOfWork.SaveChanges();
            return Json(result);
        }

        [ValidateInput(false)]
        public ActionResult Update(string models)
        {
            var deviceBOViewModel = JsonConvert.DeserializeObject<DeviceGridBOViewModel>(models);
            var device = Mapper.Map<Device>(deviceBOViewModel);
            deviceService.Update(device);
            var result = unitOfWork.SaveChanges();
            return Json(result);
        }

        public ActionResult Create(int? deviceId)
        {
            var deviceBOViewModel = new DeviceBOViewModel();
            return View(deviceBOViewModel);
        }

        [HttpPost]
        public ActionResult CreateOrUpdate(DeviceBOViewModel deviceBOViewModel)
        {
            var deviceId = 0;
            if(deviceBOViewModel.DeviceId != 0)
            {

            }
            else
            {
                var newDevice = Mapper.Map<Device>(deviceBOViewModel);
                deviceId = deviceBOViewModel.DeviceId;

                newDevice.CreatedDate = DateTime.Now;
                newDevice.IsHot = false;
                newDevice.IsDeleted = false;
                newDevice.IsActive = true;
                newDevice.ViewCount = 0;

                deviceService.Insert(newDevice);
                unitOfWork.SaveChanges();
            }
            return Json(false);
        }

        public ActionResult GetSpecs()
        {
            var specs = specService.Query().Include(a => a.SpecGroup).Select();
            return Json(specs.Select(Mapper.Map<SpecDetailGridBOViewModel>), JsonRequestBehavior.AllowGet);
        }

    }
}
