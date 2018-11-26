using AppFormBoletaGarantia.Filters;
using System.Web.Mvc;

namespace AppFormBoletaGarantia
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new BrowserFilter());
            filters.Add(new IdentityFilter());
        }
    }
}
