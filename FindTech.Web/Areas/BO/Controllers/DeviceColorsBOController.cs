using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using FindTech.Services;
using FindTech.Web.Areas.BO.Models;
using Repository.Pattern.UnitOfWork;

namespace FindTech.Web.Areas.BO.Controllers
{
    public class DeviceColorsBOController : Controller
    {

        private IDeviceColorsService deviceColorsService { get; set; }
        // GET: BO/DeviceColorsBO
        private IUnitOfWorkAsync unitOfWork { get; set; }

        public DeviceColorsBOController(IDeviceColorsService deviceColorsService, IUnitOfWorkAsync unitOfWork)
        {
            this.deviceColorsService = deviceColorsService;
            this.unitOfWork = unitOfWork;
        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _DeviceColorsForm(int? deviceColorId, int deviceId)
        {
            var deviceColors = new DeviceColorsBOViewModel { DeviceId = deviceId };
            if (deviceColorId != null)
            {
                deviceColors = Mapper.Map<DeviceColorsBOViewModel>(deviceColorsService.Queryable().Include(a => a.DeviceImages).FirstOrDefault(a => a.DeviceColorId == deviceColorId));
            }
            return View(deviceColors);
        }
    }
}