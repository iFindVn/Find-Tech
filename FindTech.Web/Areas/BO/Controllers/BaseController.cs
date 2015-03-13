using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FindTech.Entities.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace FindTech.Web.Areas.BO.Controllers
{
    public class BaseController : Controller
    {
        private FindTechUserManager _userManager;
        public FindTechUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<FindTechUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public FindTechUser CurrentUser
        {
            get
            {
                if (HttpContext.User == null || !HttpContext.User.Identity.IsAuthenticated)
                    return null;
                if (Session["CurrentUser"] == null)
                {
                    Session["CurrentUser"] = UserManager.FindById(User.Identity.GetUserId());
                }
                return (FindTechUser)Session["CurrentUser"];
            }

            set { Session["CurrentUser"] = value; }
        }
    }
}