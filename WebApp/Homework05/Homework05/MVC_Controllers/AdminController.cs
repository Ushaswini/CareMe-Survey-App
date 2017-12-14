using Homework05.Models;
using Microsoft.Owin.Security;
using System;
using System.Web;
using System.Web.Mvc;

namespace Homework_04.Controllers
{
    [AuthorizationFilter]
    public class AdminController : Controller
    {
        private IAuthenticationManager AuthenticationManager { get { return HttpContext.GetOwinContext().Authentication; } }

        public ActionResult Dashboard()
        {
            ViewBag.Title = "Dashboard";
            return View("~/Views/Admin/Dashboard.cshtml");

        }

        public ActionResult ManageResource()
        {

           // ViewBag.Title = "ManageResource";
            return View("~/Views/Admin/ManageResource.cshtml");
           // return RedirectToAction("Dashboard", "Admin");

        }

        public ActionResult PublishSurveys()
        {

            ViewBag.Title = "PublishSurveys";
            return View("~/Views/Admin/PublishSurveys.cshtml");

        }

        public ActionResult AnalyseResponses()
        {
            ViewBag.Title = "AnalyseResponses";
            return RedirectToAction("Manage", "Survey");

        }
    }
}
