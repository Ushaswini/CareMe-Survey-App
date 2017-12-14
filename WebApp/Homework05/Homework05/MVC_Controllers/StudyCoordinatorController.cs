using Homework05.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Homework05.MVC_Controllers
{
    [AuthorizationFilter]
    public class StudyCoordinatorController : Controller
    {

        public ActionResult Dashboard()
        {
            if (Session["userRole"].Equals("Admin"))
                return RedirectToAction("Dashboard", "Admin");

            ViewBag.Title = "Dashboard";
            return View();

        }
    }
}