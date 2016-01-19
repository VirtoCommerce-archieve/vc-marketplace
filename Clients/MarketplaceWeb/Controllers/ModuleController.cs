using MarketplaceWeb.Models;
using MarketplaceWeb.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MarketplaceWeb.Helpers;
using VirtoCommerce.Client.Model;

namespace MarketplaceWeb.Controllers
{
	[RoutePrefix("modules")]
	public class ModuleController : ControllerBase
	{
		//[OutputCache(Location = System.Web.UI.OutputCacheLocation.Server, Duration = 3600)]
		[Route("{id}")]
		public ActionResult Module(string id)
		{
            var product = CatalogClient.CatalogModuleProductsGet(id);

            var category = CatalogClient.CatalogModuleCategoriesGet(product.CategoryId);

			var module = product.ToWebModel();

            //var reviews = new List<VirtoCommerceMerchandisingModuleWebModelReview>(); //MerchandisingClient.MerchandisingModuleReviewGetProductReviews();
            //module.Reviews.AddRange(reviews.Select(i => i.ToWebModel(module.Keyword)));

            var vendor = CustomerServiceClient.CustomerModuleGetContactById(module.UserId);
			module.Vendor = vendor.ToWebModel();
			module.CategoryList.Add(category.Code, category.Name);

			//foreach (var review in reviews.Select(x => x.ToWebModel(product.Id)))
			//{
			//	module.Reviews.Add(review);
			//}

			if (product.SeoInfos.Any())
			{
				ViewBag.Title = product.SeoInfos[0].PageTitle;
				ViewBag.Description = product.SeoInfos[0].MetaDescription;
			}

			return View(module);
		}
	}
}