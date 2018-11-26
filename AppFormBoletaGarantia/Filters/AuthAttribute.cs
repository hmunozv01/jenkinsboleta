using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppFormBoletaGarantia.Class;
using System.Data.Entity.Infrastructure;

namespace AppFormBoletaGarantia.Filters
{
    public class AuthAttribute : ActionFilterAttribute
    {
        public int[] Groups { get; set; }
        public string[] Permissions { get; set; }

        public AuthAttribute(params int[] groups)
        {
            Groups = groups;
        }

        public AuthAttribute(params string[] permissions)
        {
            Permissions = permissions;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Groups != null && Groups.Length > 0 && !AuthHelper.Is(Groups))
            {
                throw new HttpException(401, "No puede acceder a este recurso.");
            }

            if (Permissions != null && Permissions.Length > 0 && !AuthHelper.Has(Permissions))
            {
                throw new HttpException(401, "No puede acceder a este recurso.");
            }
        }

    }
}