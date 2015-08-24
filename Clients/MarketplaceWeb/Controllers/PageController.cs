using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MarketplaceWeb.Converters;
using MarketplaceWeb.Helpers;

namespace MarketplaceWeb.Controllers
{
	using MarketplaceWeb.Models;
	using System.Threading.Tasks;
	using System.Web.Mvc;
	using VirtoCommerce.ApiClient.DataContracts;

    [RoutePrefix("pages")]
    public class PageController : ControllerBase
    {
		//[OutputCache(Location = System.Web.UI.OutputCacheLocation.Server, Duration = 3600)]
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

			var retVal = Modules(new BrowseQuery { SortProperty = "CreatedDate", SortDirection = "asc" });

			return View(retVal);
        }

        [Route("{pagename}")]
        public ActionResult DisplayPage(string pageName)
        {
            return View(pageName);
        }

		// GET: Module
		private ModulesModel Modules(BrowseQuery query)
		{
			var products = Task.Run(() => SearchClient.GetProductsAsync(StoreName, Locale, query, ItemResponseGroups.ItemLarge)).Result;

			var retVal = new ModulesModel();
			retVal.Items.AddRange(products.Items.Select(i => i.ToWebModel()));

			foreach (var item in retVal.Items)
			{
				var reviews = new ResponseCollection<Review>(); //Task.Run(() => ReviewsClient.GetReviewsAsync(item.Keyword)).Result;
				item.Reviews.AddRange(reviews.Items.Select(i => i.ToWebModel(item.Keyword)));
			}

			return retVal;
		}
    }
}