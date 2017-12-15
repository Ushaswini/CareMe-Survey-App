using Homework05.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Homework05.MVC_Controllers
{
    
    public class UserController : Controller
    {
        // GET: User
        [AuthorizationFilter("Admin")]
        public ActionResult Index()
        {
            ViewBag.Title = "Message Manager";
            return View();
        }

        [AuthorizationFilter("StudyCoordinator,Admin")]
        public ActionResult List()
        {
            ViewBag.Title = "User Manager";
            return View();
        }
    }
}