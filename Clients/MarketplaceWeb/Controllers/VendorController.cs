using MarketplaceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MarketplaceWeb.Converters;
using MarketplaceWeb.Helpers;
using VirtoCommerce.Client.Model;

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
		public ActionResult Information(string vendorId, BrowseQuery query)
		{
            var vendor = CustomerServiceClient.CustomerModuleGetContactById(vendorId);
            var model = vendor.ToWebModel();

            if(query == null)
            {
                query = new BrowseQuery();
            }
			query.Filters.Add("vendorId", new[] { vendorId });
			query.Take = 50;
            query.ItemResponseGroup = "ItemLarge";
			var results = GetProducts(query);

			foreach (var module in results.Select(x => x.ToWebModel()))
			{
                var reviews = new List<VirtoCommerceMerchandisingModuleWebModelReview>(); //MerchandisingClient.MerchandisingModuleReviewGetProductReviews();
                module.Reviews.AddRange(reviews.Select(i => i.ToWebModel(module.Keyword)));

				model.Modules.Add(module);
			}

			ViewBag.Title = model.Seo.Title;
			ViewBag.MetaDescription = model.Seo.MetaDescription;

			return View(model);
		}
	}
}