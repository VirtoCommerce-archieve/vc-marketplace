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

namespace MarketplaceWeb.Controllers
{
	[RoutePrefix("modules")]
	public class ModuleController : ControllerBase
	{
		public ActionResult MainModules()
		{
			var retVal = Modules(new BrowseQuery { SortProperty = "CreatedDate", SortDirection = "asc" });

			return PartialView(retVal);
		}

		[Route("{keyword}")]
		public ActionResult Module(string keyword)
		{
			var product = Task.Run(() => SearchClient.GetProductByKeywordAsync("MarketPlace", "en-US", keyword, ItemResponseGroups.ItemLarge)).Result;
			var reviews = Task.Run(() => ReviewsClient.GetReviewsAsync(product.Code)).Result;
			var category = Task.Run(() => SearchClient.GetCategoryAsync("MarketPlace", "en-US", product.Categories.Last())).Result;

			var module = product.ToWebModel();

			var vendor = Task.Run(() => CustomerServiceClient.GetContactByIdAsync(module.UserId)).Result;
			module.Vendor = vendor.ToWebModel();
			module.CategoryList.Add(category.Code, category.Name);

			foreach (var review in reviews.Items.Select(x => x.ToWebModel(product.Id)))
			{
				module.Reviews.Add(review);
			}

			if (product.Seo.Any())
			{
				ViewBag.Title = product.Seo[0].Title;
				ViewBag.Description = product.Seo[0].MetaDescription;
			}

			return View(module);
		}


		// GET: Module
		private ModulesModel Modules(BrowseQuery query)
		{
			var products = Task.Run(() => SearchClient.GetProductsAsync("MarketPlace", "en-US", query, ItemResponseGroups.ItemLarge)).Result;

			var retVal = new ModulesModel();
			retVal.Items.AddRange(products.Items.Select(i => i.ToWebModel()));

			foreach (var item in retVal.Items)
			{
				var reviews = Task.Run(() => ReviewsClient.GetReviewsAsync(item.Keyword)).Result;
				item.Reviews.AddRange(reviews.Items.Select(i => i.ToWebModel(item.Keyword)));
			}

			return retVal;
		}
	}
}