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
        private string userRoles;

        public AuthorizationFilter(string _userRoles) {
            this.userRoles = _userRoles;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string currentUserRole = filterContext.HttpContext.Session["userRole"] != null ? filterContext.HttpContext.Session["userRole"].ToString() : null;

            if (filterContext.HttpContext.Session["accessToken"] == null || currentUserRole == null)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "controller", "Home" },{ "action" , "index"} });
                /*filterContext.Result = new ContentResult()
                {
                    Content = "Unauthorized to access specified resource."
                };*/
            }

            if (!userRoles.Split(',').Contains(currentUserRole))
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "controller", currentUserRole }, { "action", "Dashboard" } });
                /*filterContext.Result = new ContentResult()
                {
                    Content = "Unauthorized to access specified resource."
                };*/
            }
            base.OnActionExecuting(filterContext);
        }
    }
}