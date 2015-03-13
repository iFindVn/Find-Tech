using AutoMapper;
using FindTech.Web.Areas.BO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FindTech.Services;
using Repository.Pattern.UnitOfWork;

namespace FindTech.Web.Areas.BO.Controllers
{
    public class DeviceBOController : Controller
    {
        private IDeviceService deviceService { get; set; }
        private IUnitOfWorkAsync unitOfWork { get; set; }

        //public DeviceBOController(IDeviceService deviceService, IUnitOfWorkAsync unitOfWork)
        //{
        //    this.deviceService = deviceService;
        //    this.unitOfWork = unitOfWork;
        //}
        // GET: BO/DevicesBO
        public ActionResult Index()
        {
            return View();
        }

        // GET: BO/DevicesBO/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BO/DevicesBO/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BO/DevicesBO/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: BO/DevicesBO/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BO/DevicesBO/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: BO/DevicesBO/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BO/DevicesBO/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
