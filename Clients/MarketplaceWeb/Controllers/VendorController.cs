using MarketplaceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MarketplaceWeb.Converters;
using MarketplaceWeb.Helpers;
using VirtoCommerce.ApiClient.DataContracts;
using VirtoCommerce.ApiClient.Extensions;

namespace MarketplaceWeb.Controllers
{
	[RoutePrefix("vendor")]
	public class VendorController : ControllerBase
	{
		// GET: Vendor
		public ActionResult Index()
		{
			return View();
		}

		[Route("{vendorId}")]
		public async Task<ActionResult> Information(string vendorId, BrowseQuery query)
		{
			var model = new Vendor();

			var userHelper = new UserHelper();

			model = await userHelper.GetUser(vendorId);

			query.Filters.Add("vendorId", new[] { vendorId });
			var results = await SearchClient.GetProductsAsync("MarketPlace", "en-US", query, ItemResponseGroups.ItemLarge);

			foreach(var module in results.Items.Select(x => x.ToWebModel()))
			{
				var reviews = await ReviewsClient.GetReviewsAsync(module.Code);
				module.Reviews.AddRange(reviews.Items.Select(r => r.ToWebModel(module.Code)));

				model.Modules.Add(module);
			}

			ViewBag.Title = model.Seo.Title;
			ViewBag.MetaDescription = model.Seo.MetaDescription;

			return View(model);
		}
	}
}