using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using FindTech.Entities.Models;
using FindTech.Services;
using FindTech.Web.Models;
using Repository.Pattern.UnitOfWork;

namespace FindTech.Web.Controllers
{
    public class LikeController : Controller
    {
        private ILikeService likeService { get; set; }
        private IUnitOfWorkAsync unitOfWork { get; set; }

        public LikeController(ILikeService likeService, IUnitOfWorkAsync unitOfWork)
        {
            this.likeService = likeService;
            this.unitOfWork = unitOfWork;
        }
        // GET: Like
        public ActionResult Create(LikeViewModel newLike)
        {
            var like = Mapper.Map<Like>(newLike);
            likeService.Insert(like);
            unitOfWork.SaveChanges();
            var likeCount = likeService.GetLikeCount(newLike.ObjectId, newLike.ObjectType);
            var likedCommentIds = (List<int>)Session["LikedCommentIds"];
            likedCommentIds.Add(newLike.ObjectId);
            return Json(new { like = Mapper.Map<LikeViewModel>(like), likeCount }, JsonRequestBehavior.AllowGet);
        }
    }
}