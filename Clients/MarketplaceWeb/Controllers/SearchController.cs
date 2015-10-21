using System.Diagnostics;
using System.Net;
using MarketplaceWeb.Converters;
using MarketplaceWeb.Helpers;
using MarketplaceWeb.Helpers.Marketing;
using MarketplaceWeb.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using VirtoCommerce.Client.Model;

namespace MarketplaceWeb.Controllers
{
	[RoutePrefix("search")]
	public class SearchController : ControllerBase
	{
		//[OutputCache(Location = System.Web.UI.OutputCacheLocation.Server, Duration = 3600)]
		[Route("ven")]
		public ActionResult DeveloperExtensions(string vendorId, string sort)
		{
			var query = new BrowseQuery();
			query.Filters = new Dictionary<string, string[]>();
			query.Filters.Add("vendorId", new[] { vendorId });
			query.Take = 1000;
            query.ItemResponseGroup = "ItemLarge";
			var results = GetProducts(query);

			DeveloperSearchResult retVal = new DeveloperSearchResult();
			retVal.Results.AddRange(results.Select(i => i.ToWebModel()));
			foreach (var module in retVal.Results)
			{
                var reviews = new List<VirtoCommerceMerchandisingModuleWebModelReview>(); //MerchandisingClient.MerchandisingModuleReviewGetProductReviews();
                module.Reviews.AddRange(reviews.Select(i => i.ToWebModel(module.Keyword)));
            }

			var customer = CustomerServiceClient.CustomerModuleGetContactById(vendorId);
			retVal.VenderName = customer.FullName;
			retVal.VendorId = vendorId;

			return View(retVal);
		}

		//[OutputCache(Location = System.Web.UI.OutputCacheLocation.Server, Duration = 3600, VaryByParam = "q")]
		[Route("term")]
		[HttpGet]
		public ActionResult Search(string q)
		{
			var query = new BrowseQuery
			{
				Search = q.EscapeSearchTerm(),
				Take = 1000,
                ItemResponseGroup = "ItemLarge"
			};
			var results = GetProducts(query);

			SearchResult retVal = new SearchResult();
			retVal.Results.AddRange(results.Select(i => i.ToWebModel()));
			foreach (var module in retVal.Results)
			{
                var reviews = new List<VirtoCommerceMerchandisingModuleWebModelReview>(); //MerchandisingClient.MerchandisingModuleReviewGetProductReviews();
                module.Reviews.AddRange(reviews.Select(i => i.ToWebModel(module.Keyword)));
            }

			retVal.SearchTerm = q;

			return View(retVal);
		}

		[Route("find")]
		public ActionResult Find(string q)
		{
			var query = new BrowseQuery
			{
				Take = 15,
				Search = q.EscapeSearchTerm()
			};
			var results = GetProducts(query);

			var data = from i in results
					   select new
					   {
						   url = string.Format(SiteUrlHelper.ResolveServerUrl("~/modules/{0}"), i.Code),
						   value = i.Name
					   };

			return Json(data.ToArray(), JsonRequestBehavior.AllowGet);
		}
	}
}