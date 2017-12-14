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

        public ActionResult ManageResource()
        {
            return View();
        }

        public ActionResult PublishSurveys()
        {

            ViewBag.Title = "PublishSurveys";
            return View();

        }

        public ActionResult AnalyseResponses()
        {
            ViewBag.Title = "AnalyseResponses";
            return RedirectToAction("Manage", "Survey");

        }

        public ActionResult ManageSurveys() {
            ViewBag.Title = "Manage Surveys";
            return View();
        }
    }
}