using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MarketplaceWeb.Converters;
using MarketplaceWeb.Helpers;
using VirtoCommerce.Client.Model;

namespace MarketplaceWeb.Controllers
{
	using MarketplaceWeb.Models;
	using System.Threading.Tasks;
	using System.Web.Mvc;

    [RoutePrefix("pages")]
    public class PageController : ControllerBase
    {
		//[OutputCache(Location = System.Web.UI.OutputCacheLocation.Server, Duration = 3600)]
        public ActionResult Index()
        {
			var stores = StoreClient.StoreModuleGetStores();
			var store = stores.FirstOrDefault(s => s.Name == "MarketPlace");

			if(store != null && store.DynamicProperties != null && store.DynamicProperties.Any())
			{
				var titleProperty = store.DynamicProperties.FirstOrDefault(s => s.Name == "Title");
				var metaDescriptionProperty = store.DynamicProperties.FirstOrDefault(s => s.Name == "MetaDescription");
                if (titleProperty != null)
                {
                    ViewBag.Title = titleProperty.Values.FirstOrDefault();
                }
                if (metaDescriptionProperty != null)
                {
                    ViewBag.MetaDescription = metaDescriptionProperty.Values.FirstOrDefault();
                }
			}

			var retVal = Modules(new BrowseQuery { SortProperty = "CreatedDate", SortDirection = "asc", Skip = 0, Take = 10 });

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
            var products = GetProducts(query);

            var retVal = new ModulesModel();
			retVal.Items.AddRange(products.Select(i => i.ToWebModel()));

			//foreach (var item in retVal.Items)
			//{
   //             var reviews = new List<VirtoCommerceMerchandisingModuleWebModelReview>(); //MerchandisingClient.MerchandisingModuleReviewGetProductReviews();
   //             item.Reviews.AddRange(reviews.Select(i => i.ToWebModel(item.Keyword)));
   //         }

			return retVal;
		}
    }
}