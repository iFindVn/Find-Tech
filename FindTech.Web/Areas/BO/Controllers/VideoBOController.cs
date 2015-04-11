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
    public class VideoBOController : Controller
    {
        private IVideoService videoService { get; set; }
        private IUnitOfWorkAsync unitOfWork { get; set; }

        public VideoBOController(IVideoService videoService, IUnitOfWorkAsync unitOfWork)
        {
            this.videoService = videoService;
            this.unitOfWork = unitOfWork;
        }
        // GET: BO/VideoBO
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetVideos()
        {
            var videos = videoService.Query().Select();
            return Json(videos.Select(Mapper.Map<VideoBOModel>), JsonRequestBehavior.AllowGet);
        }
    }
}