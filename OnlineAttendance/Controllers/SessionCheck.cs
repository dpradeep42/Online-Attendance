using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OnlineAttendance.Controllers
{
    public class AdminCheck : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            if (session["AdminStart"] == null)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    //string errormsg = "NOT found";
                    filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary {
                                { "Controller", "Home" },
                                { "Action", "Index" }
                            });
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary {
                                { "Controller", "Home" },
                                { "Action", "Index" }
                            });
                }
            }
        }
    }
    public class FacultyCheck : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            if (session["FacultyStart"] == null)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    //string errormsg = "NOT found";
                    filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary {
                                { "Controller", "Faculty" },
                                { "Action", "Index" }
                            });
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary {
                                { "Controller", "Faculty" },
                                { "Action", "Index" }
                            });
                }
            }
        }
    }
}