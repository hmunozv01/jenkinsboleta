using System.Web;
using System.Web.Optimization;

namespace AppFormBoletaGarantia
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                        "~/Scripts/vue.js",
                        "~/Scripts/vue-mixins.js",
                        "~/Scripts/jquery-1.10.2.min.js",
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/jquery.validate.unobtrusive.min.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/moment-with-locales.js",
                        "~/Scripts/mobile-detect.min.js",
                        "~/Scripts/daterangepicker.js",
                        "~/Scripts/underscore-min.js",
                        "~/Scripts/phpjs.js",
                        "~/Scripts/utils.js"
            ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/Stylesheets/bootstrap.css",
                      "~/Content/Stylesheets/bootstrap-select.min.css",
                      "~/Content/Stylesheets/daterangepicker.css",
                      "~/Content/Stylesheets/Site.css"
            ));
        }
    }
}
