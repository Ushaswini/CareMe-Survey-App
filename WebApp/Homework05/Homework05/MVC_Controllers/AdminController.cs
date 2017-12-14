using Homework05.Models;
using Microsoft.Owin.Security;
using System;
using System.Web;
using System.Web.Mvc;

namespace Homework05.MVC_Controllers
{
    [AuthorizationFilter("Admin")]
    public class AdminController : Controller
    {
        private IAuthenticationManager AuthenticationManager { get { return HttpContext.GetOwinContext().Authentication; } }

        public ActionResult Dashboard()
        {
            ViewBag.Title = "Dashboard";
            return View("~/Views/Admin/Dashboard.cshtml");

        }
        
        public ActionResult AnalyseResponses()
        {
            ViewBag.Title = "AnalyseResponses";
            return RedirectToAction("Manage", "Survey");

        }
    }
}
