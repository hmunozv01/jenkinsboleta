using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AppFormBoletaGarantia.Filters
{
    public class BrowserFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "Main" && filterContext.ActionDescriptor.ActionName == "Incompatible")
            {
                // nada
            }
            else
            {
                HttpRequestBase request = filterContext.HttpContext.Request;
                if ((request.Browser.Browser == "IE" || request.Browser.Browser == "InternetExplorer") && ((request.Browser.MajorVersion < 9)))
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                    {
                        { "controller", "Main" },
                        { "action", "Incompatible" }
                    });
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}