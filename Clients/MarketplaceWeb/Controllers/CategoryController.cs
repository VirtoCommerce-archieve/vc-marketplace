using MarketplaceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.Client.Model;
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
			var result = CatalogClient.CatalogModuleSearchSearch(criteriaCatalogId: Store.Catalog, criteriaResponseGroup: "WithCategories");

			var retVal = new CategoryMenu();

			foreach (var category in result.Categories.Where(c => c.IsActive ?? true))
			{
				retVal.Categories.Add(new ShortCategoryInfo
				{
					CategoryCode = category.SeoInfos?.FirstOrDefault()?.SemanticUrl ?? category.Id,
					CategoryName = category.Name
				});
			}

			return PartialView(retVal);
		}

		//[OutputCache(Location = System.Web.UI.OutputCacheLocation.Server, Duration = 3600)]
		[Route("{id}")]
		public ActionResult Category(string id)
		{
			CategoryResults retVal = new CategoryResults();

            var category = CatalogClient.CatalogModuleCategoriesGet(id);                

			if (category != null)
			{
                if(category.IsActive.HasValue && !category.IsActive.Value)
                {
                    return Redirect("~");
                }

				var query = new BrowseQuery();
				query.Filters = new Dictionary<string, string[]>();
				query.CategoryId = id;
				query.Take = 50;
                query.ItemResponseGroup = "21";

				var products = GetProducts(query);
				retVal.Modules.AddRange(products.Select(i => i.ToWebModel()));
				//foreach (var module in retVal.Modules)
				//{
				//	var reviews = new List<VirtoCommerce>(); //MerchandisingClient.MerchandisingModuleReviewGetProductReviews();
    //                module.Reviews.AddRange(reviews.Select(i => i.ToWebModel(module.Keyword)));
				//}

				retVal.CategoryCode = category.Code;
				retVal.CategoryName = category.Name;
			}

			if(category.SeoInfos.Any())
			{
				ViewBag.Title = category.SeoInfos.First().PageTitle;
				ViewBag.Description = category.SeoInfos.First().MetaDescription;
			}

			return View(retVal);
		}

		private string GetOutline(VirtoCommerceCatalogModuleWebModelCategory category)
		{
			var ids = category.Parents != null ? category.Parents.Select(x => x.Key).ToList() : new List<string>();
			ids.Add(category.Id);
			return string.Join("/", ids);
		}
	}
}