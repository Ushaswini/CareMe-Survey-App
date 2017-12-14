using Homework05.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Homework05.MVC_Controllers
{
    [AuthorizationFilter("StudyCoordinator")]
    public class StudyCoordinatorController : Controller
    {

        public ActionResult Dashboard()
        {

            ViewBag.Title = "Dashboard";
            return View();

        }
    }
}