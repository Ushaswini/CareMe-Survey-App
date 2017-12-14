using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.Http.Filters;
using System.Web.Mvc;

namespace Homework05.Models
{
    public class AuthorizationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["accessToken"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "controller", "Home" },{ "action" , "index"} });
                /*filterContext.Result = new ContentResult()
                {
                    Content = "Unauthorized to access specified resource."
                };*/
            }
            base.OnActionExecuting(filterContext);
        }
    }
}