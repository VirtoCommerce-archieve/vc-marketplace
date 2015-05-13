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
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.DataContracts;
using VirtoCommerce.ApiClient.Extensions;
using System.Collections.Generic;

namespace MarketplaceWeb.Controllers
{
	[RoutePrefix("search")]
	public class SearchController : ControllerBase
	{
		[Route("ven")]
		public async Task<ActionResult> DeveloperExtensions(string vendorId, string sort)
		{
			var query = new BrowseQuery();
			query.Filters = new Dictionary<string, string[]>();
			query.Filters.Add("vendorId", new[] { vendorId });
			query.Take = 1000;
			var results = await SearchClient.GetProductsAsync("MarketPlace", "en-US", query, ItemResponseGroups.ItemLarge);

			DeveloperSearchResult retVal = new DeveloperSearchResult();
			retVal.Results.AddRange(results.Items.Select(i => i.ToWebModel()));
			foreach (var module in retVal.Results)
			{
				var reviews = await ReviewsClient.GetReviewsAsync(module.Keyword);
				module.Reviews.AddRange(reviews.Items.Select(i => i.ToWebModel(module.Keyword)));
			}

			var customer = await CustomerServiceClient.GetContactByIdAsync(vendorId);
			retVal.VenderName = customer.FullName;
			retVal.VendorId = vendorId;

			return View(retVal);
		}

		[Route("term")]
		[HttpGet]
		public async Task<ActionResult> Search(string q)
		{
			var query = new BrowseQuery
			{
				Search = q.EscapeSearchTerm(),
				Take = 1000
			};
			var results = await SearchClient.GetProductsAsync("MarketPlace", "en-US", query, ItemResponseGroups.ItemLarge);

			SearchResult retVal = new SearchResult();
			retVal.Results.AddRange(results.Items.Select(i => i.ToWebModel()));
			foreach (var module in retVal.Results)
			{
				var reviews = await ReviewsClient.GetReviewsAsync(module.Keyword);
				module.Reviews.AddRange(reviews.Items.Select(i => i.ToWebModel(module.Keyword)));
			}

			retVal.SearchTerm = q;

			return View(retVal);
		}

		[Route("find")]
		public async Task<ActionResult> Find(string q)
		{
			var query = new BrowseQuery
			{
				Take = 15,
				Search = q.EscapeSearchTerm()
			};
			var results = await SearchClient.GetProductsAsync("MarketPlace", "en-US", query);

			var data = from i in results.Items
					   select new
					   {
						   url = string.Format(SiteUrlHelper.ResolveServerUrl("~/modules/{0}"), i.Code),
						   value = i.Name
					   };
			return Json(data.ToArray(), JsonRequestBehavior.AllowGet);
		}
	}
}