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

        [HttpPost]
        public ActionResult Create(string models)
        {
            List<VideoBOModel> videoBOModels = JsonConvert.DeserializeObject<List<VideoBOModel>>(models);
            for (var i = 0; i < videoBOModels.Count; i++)
            {
                VideoBOModel videoBOModel = videoBOModels.ElementAt(i);
                Video video = Mapper.Map<Video>(videoBOModel);
                videoService.Insert(video);
                unitOfWork.SaveChanges();
                videoBOModels.RemoveAt(i);
                videoBOModels.Add(Mapper.Map<VideoBOModel>(video));
            }
            return Json(videoBOModels, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(string videoBOModel)
        {
            return Json(true);
        }

        [HttpPost]
        public ActionResult Destroy(string videoBOModel)
        {
            return Json(true);
        }
    }
}