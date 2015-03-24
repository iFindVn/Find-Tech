using AutoMapper;
using FindTech.Entities.Models;
using FindTech.Entities.Models.Enums;
using FindTech.Services;
using FindTech.Web.Models;
using Newtonsoft.Json;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FindTech.Web.Controllers
{
    public class CommentController : Controller
    {
        private ICommentService commentService { get; set; }
        private ILikeService likeService { get; set; }
        private IUnitOfWorkAsync unitOfWork { get; set; }

        public CommentController(ICommentService commentService, ILikeService likeService, IUnitOfWorkAsync unitOfWork)
        {
            this.commentService = commentService;
            this.likeService = likeService;
            this.unitOfWork = unitOfWork;
        }
        // GET: Comment
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetComments(ObjectType objectType, int objectId)
        {
            var comments =
                commentService.GetComments(objectId, objectType).ToList().Select(a => new CommentModel
                    {
                        CommentId = a.CommentId,
                        CommentatorEmail = a.CommentatorEmail,
                        Content = a.Content,
                        CreatedDate = a.CreatedDate,
                        LikeCount = likeService.GetLikeCount(a.CommentId, ObjectType.Comment),
                        ObjectId = a.ObjectId,
                        ObjectType = a.ObjectType,
                        Replies =
                            commentService.GetReplies(a.CommentId)
                                .Select(Mapper.Map<CommentModel>)
                    });

            return Json(comments, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create(CommentModel newComment)
        {
                var comment = Mapper.Map<Comment>(newComment);
                commentService.Insert(comment);
                unitOfWork.SaveChanges();
                return Json(comment, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Update(string comment)
        {
            return Json(false);
        }
        public ActionResult Delete(string comment)
        {
            return Json(false);
        }
    }
}