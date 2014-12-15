using System.Web;
using System.Web.Optimization;

namespace MarketplaceWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jquerymisc").Include(
            "~/Scripts/v/virto-jquery.js",
            //"~/Scripts/cloudzoom.js",
           // "~/Scripts/ajaxq.js",
            //"~/Scripts/jquery.rateit.js",
            "~/Scripts/v/virto-commerce.js",
            "~/Scripts/responsive/responsive.js",
            "~/Scripts/v/validation.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));


            bundles.Add(new ScriptBundle("~/bundles/responsive").Include(
                "~/Scripts/responsive/responsive.js"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                    "~/Content/themes/base/core.css",
                    "~/Content/themes/base/resizable.css",
                    "~/Content/themes/base/selectable.css",
                    "~/Content/themes/base/accordion.css",
                    "~/Content/themes/base/autocomplete.css",
                    "~/Content/themes/base/button.css",
                    "~/Content/themes/base/dialog.css",
                    "~/Content/themes/base/slider.css",
                    "~/Content/themes/base/tabs.css",
                    "~/Content/themes/base/datepicker.css",
                    "~/Content/themes/base/progressbar.css",
                    "~/Content/themes/base/theme.css",
                    "~/Content/themes/base/tooltip.css",
                    "~/Content/themes/base/spinner.css",
                    "~/Content/themes/base/menu.css"));

            bundles.Add(new StyleBundle("~/Content/themes/default/css/css").Include(
                      "~/Content/themes/default/css/reset.css",
                      "~/Content/themes/default/css/base_modules.css",
                      "~/Content/themes/default/css/project_modules.css",
                      "~/Content/themes/default/css/responsive.css",
                      "~/Content/themes/default/css/cosmetic.css"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            //BundleTable.EnableOptimizations = false;
        }
    }
}
