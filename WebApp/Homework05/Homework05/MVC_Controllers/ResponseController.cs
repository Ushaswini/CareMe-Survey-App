﻿using Homework05.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Homework_04.Controllers
{
    [AuthorizationFilter]
    public class ResponseController : Controller
    {
        
        // GET: Response
        public ActionResult Index()
        {
            return View();
        }
    }
}