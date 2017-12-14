using Homework05.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace Homework05.MVC_Controllers
{
    [AuthorizationFilter("StudyCoordinator")]
    public class SurveyController : Controller
    {
        public ActionResult Manage()
        {
            ViewBag.Title = "Survey Manager";

            return View("~/Views/Survey/Manage.cshtml");
        }
    }
}
