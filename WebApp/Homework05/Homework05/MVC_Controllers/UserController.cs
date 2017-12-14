using Homework05.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Homework05.MVC_Controllers
{
    [AuthorizationFilter("Admin")]
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            ViewBag.Title = "Message Manager";
            return View();
        }
    }
}