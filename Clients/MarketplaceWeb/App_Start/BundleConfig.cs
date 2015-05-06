using System.Web;
using System.Web.Optimization;

namespace MarketplaceWeb
{
	public class BundleConfig
	{
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/js/main").Include(
						"~/Scripts/main.js",
						"~/Scripts/modernizr.js",
						"~/Scripts/slider.js",
						"~/Scripts/new.js"));

			bundles.Add(new StyleBundle("~/bundles/css/main").Include(
					  "~/Content/css/reset.css",
					  "~/Content/css/base-modules.css",
					  "~/Content/css/project-modules.css",
					  "~/Content/css/responsive.css",
					  "~/Content/css/cosmetic.css"));


			BundleTable.EnableOptimizations = false;
		}
	}
}
