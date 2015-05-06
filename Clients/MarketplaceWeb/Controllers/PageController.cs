using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarketplaceWeb.Controllers
{
	using System.Threading.Tasks;
	using System.Web.Mvc;

    [RoutePrefix("pages")]
    public class PageController : ControllerBase
    {
        public async Task<ActionResult> Index()
        {
			var stores = await StoreClient.GetStoresAsync();
			var store = stores.FirstOrDefault(s => s.Name == "MarketPlace");

			if(store != null && store.Settings.Any())
			{
				var title = store.Settings.FirstOrDefault(s => s.Key == "Title");
				var metaDescription = store.Settings.FirstOrDefault(s => s.Key == "MetaDescription");
				ViewBag.Title = title.Value;
				ViewBag.MetaDescription = metaDescription.Value;
			}

            return View();
        }

        [Route("{pagename}")]
        public ActionResult DisplayPage(string pageName)
        {
            return View(pageName);
        }
    }
}