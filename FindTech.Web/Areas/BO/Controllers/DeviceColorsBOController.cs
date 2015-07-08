using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using FindTech.Entities.Models;
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

        private IDeviceImageService deviceImageService { get; set; }

        private IDeviceService deviceService { get; set; }

        public DeviceColorsBOController(IDeviceColorsService deviceColorsService, IUnitOfWorkAsync unitOfWork, IDeviceImageService deviceImageService, IDeviceService deviceService)
        {
            this.deviceColorsService = deviceColorsService;
            this.unitOfWork = unitOfWork;
            this.deviceImageService = deviceImageService;
            this.deviceService = deviceService;
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }



        public ActionResult DeviceColorsForm(int? deviceColorId, int deviceId)
        {
            var deviceColors = new DeviceColorsBOViewModel { DeviceId = deviceId };
            if (deviceColorId != null)
            {
                deviceColors = Mapper.Map<DeviceColorsBOViewModel>(deviceColorsService.Queryable().Include(a => a.DeviceImages).FirstOrDefault(a => a.DeviceColorId == deviceColorId));
            }
            return View(deviceColors);
        }

        [HttpPost]
        public ActionResult Create(DeviceColorsBOViewModel deviceColorsBOViewModel)
        {
            var deviceColors = Mapper.Map<DeviceColor>(deviceColorsBOViewModel);

           
            if (deviceColors.DeviceColorId != 0)
            {
                var count =
                    deviceColorsService.Queryable().Count(a => a.DeviceColorId == deviceColors.DeviceColorId);
                if (count > 0)
                {
                    deviceColorsService.Update(deviceColors);
                }
            }
            else
            {
                deviceColors.Device = deviceService.Find(deviceColors.DeviceId);
                deviceColorsService.Insert(deviceColors);
            }
            unitOfWork.SaveChanges();
            deviceColors.DeviceImages = deviceColorsService.Queryable().Include(a => a.DeviceImages).FirstOrDefault(a => a.DeviceColorId == deviceColors.DeviceColorId).DeviceImages;
            return View("DeviceColorsForm", Mapper.Map<DeviceColorsBOViewModel>(deviceColors));
        }


        [HttpPost]
        public ActionResult AddImage(DeviceImage deviceImage, int deviceColorsId)
        {
            var deviceColors = deviceColorsService.Queryable().FirstOrDefault(a => a.DeviceColorId == deviceColorsId);
            if (deviceColors != null)
            {
                deviceImageService.Insert(deviceImage);

                deviceColors.DeviceImages.Add(deviceImage);

                deviceColorsService.Update(deviceColors);

                unitOfWork.SaveChanges();
            }
            return Json(deviceImage.DeviceImageId, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteImage(int deviceImageId, int deviceColorID)
        {
            var deviceImage = deviceImageService.Queryable().FirstOrDefault(a => a.DeviceImageId == deviceImageId);
            var deviceColors =
                deviceColorsService.Queryable().FirstOrDefault(a => a.DeviceColorId == deviceColorID);
            if (deviceColors != null && deviceImage != null)
            {
                deviceImageService.Delete(deviceImage);

                deviceColors.DeviceImages.Remove(deviceImage);

                deviceColorsService.Update(deviceColors);

                unitOfWork.SaveChanges();
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}