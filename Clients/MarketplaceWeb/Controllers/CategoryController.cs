using MarketplaceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.ApiClient.DataContracts;
using MarketplaceWeb.Helpers;
using MarketplaceWeb.Converters;

namespace MarketplaceWeb.Controllers
{
	[RoutePrefix("cat")]
	public class CategoryController : ControllerBase
	{
		// GET: Category
		public ActionResult CategoryMenu()
		{
			var categories = Task.Run(() => SearchClient.GetCategoriesAsync("MarketPlace", "en-US")).Result;

			var retVal = new CategoryMenu();

			foreach (var category in categories.Items)
			{
				retVal.Categories.Add(new ShortCategoryInfo
				{
					CategoryCode = category.Code,
					CategoryName = category.Name
				});
			}

			return PartialView(retVal);
		}

		//[OutputCache(Location = System.Web.UI.OutputCacheLocation.Server, Duration = 3600)]
		[Route("{id}")]
		public async Task<ActionResult> Category(string id)
		{
			CategoryResults retVal = new CategoryResults();

			var category = await SearchClient.GetCategoryByCodeAsync("MarketPlace", "en-US", id);

			if (category != null)
			{
				var query = new BrowseQuery();
				query.Filters = new Dictionary<string, string[]>();
				query.Outline = GetOutline(category);

				var products = await SearchClient.GetProductsAsync("MarketPlace", "en-Us", query, ItemResponseGroups.ItemLarge);
				retVal.Modules.AddRange(products.Items.Select(i => i.ToWebModel()));
				foreach (var module in retVal.Modules)
				{
					var reviews = new ResponseCollection<Review>(); //await ReviewsClient.GetReviewsAsync(module.Keyword);
					module.Reviews.AddRange(reviews.Items.Select(i => i.ToWebModel(module.Keyword)));
				}

				retVal.CategoryCode = category.Code;
				retVal.CategoryName = category.Name;
			}

			if(category.Seo.Any())
			{
				ViewBag.Title = category.Seo.First().Title;
                ViewBag.Description = category.Seo.First().MetaDescription;
			}

			return View(retVal);
		}

		private string GetOutline(Category category)
		{
			var ids = category.Parents != null ? category.Parents.Select(x => x.Id).ToList() : new List<string>();
			ids.Add(category.Id);
			return string.Join("/", ids);
		}

	}
}