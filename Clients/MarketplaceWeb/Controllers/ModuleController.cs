using MarketplaceWeb.Models;
using MarketplaceWeb.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.ApiClient.DataContracts;
using MarketplaceWeb.Helpers;
using System.Diagnostics;

namespace MarketplaceWeb.Controllers
{
	[RoutePrefix("modules")]
	public class ModuleController : ControllerBase
	{
		[OutputCache(Location = System.Web.UI.OutputCacheLocation.Server, Duration = 3600)]
		[Route("{keyword}")]
		public ActionResult Module(string keyword)
		{
			var sW = Stopwatch.StartNew();
			var product = Task.Run(() => SearchClient.GetProductByKeywordAsync("MarketPlace", "en-US", keyword, ItemResponseGroups.ItemLarge)).Result;
			var timePR = sW.ElapsedMilliseconds;

			var reviews = new ResponseCollection<Review>(); //Task.Run(() => ReviewsClient.GetReviewsAsync(product.Code)).Result;
			var timeRR = sW.ElapsedMilliseconds;

			var category = Task.Run(() => SearchClient.GetCategoryAsync("MarketPlace", "en-US", product.Categories.Last())).Result;
			var timeCR = sW.ElapsedMilliseconds;

			var module = product.ToWebModel();

			var vendor = Task.Run(() => CustomerServiceClient.GetContactByIdAsync(module.UserId)).Result;
			module.Vendor = vendor.ToWebModel();
			module.CategoryList.Add(category.Code, category.Name);
			var timeVR = sW.ElapsedMilliseconds;

			foreach (var review in reviews.Items.Select(x => x.ToWebModel(product.Id)))
			{
				module.Reviews.Add(review);
			}

			if (product.Seo.Any())
			{
				ViewBag.Title = product.Seo[0].Title;
				ViewBag.Description = product.Seo[0].MetaDescription;
			}
			var allTime = sW.ElapsedMilliseconds;

			sW.Stop();
			module.Time = new List<long>();
			module.Time.AddRange(new long[] { timePR, timeRR, timeCR, timeVR, allTime });
			return View(module);
		}
	}
}