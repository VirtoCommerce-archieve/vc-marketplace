using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MarketplaceWeb.Converters;
using MarketplaceWeb.Models;
using MvcSiteMapProvider;
using VirtoCommerce.ApiClient;
using MarketplaceWeb.Helpers;
using VirtoCommerce.ApiClient.DataContracts;

namespace MarketplaceWeb.Controllers
{
	[RoutePrefix("extension")]
	public class ExtensionController : ControllerBase
	{

		[Route("{id}")]
		public async Task<ActionResult> DisplayItem(string id)
		{
			var item = await SearchClient.GetProductByCodeAsync("MarketPlace", "en-US", id, ItemResponseGroups.ItemLarge);
			var reviews = await ReviewsClient.GetReviewsAsync(id);

			if (ReferenceEquals(item, null))
			{
				throw new HttpException(404, "Item not found");
			}

			var extension = item.ToWebModel();
			var userHelper = new UserHelper();
			extension.User = await userHelper.GetUser(item);

			var model = new FullExtension
			{
				Extension = extension
			};

			if (reviews != null && reviews.TotalCount > 0)
			{
				model.Reviews = reviews.Items.Select(x => x.ToWebModel()).ToArray();
			}

			if (SiteMaps.Current != null)
			{
				var node = SiteMaps.Current.CurrentNode;
				if (node != null)
				{
					node.Title = item.Name;
				}
			}

			if (item.Seo.Any())
			{
				ViewBag.Title = item.Seo[0].Title;
				ViewBag.Description = item.Seo[0].MetaDescription;
			}

			return View(model);
		}

		//[Route("review/{id}")]
		//public async Task<ActionResult> SaveReview(string id)
		//{

		//}

		/// <summary>
		/// This method used only for dynamic content. It cannot be exectuted async due to request limitations
		/// </summary>
		[ChildActionOnly]
		public ActionResult DisplayDynamic(string itemCode)
		{
			try
			{
				var product = Task.Run(() => SearchClient.GetProductByCodeAsync("MarketPlace", "en-US", itemCode)).Result;
				var reviews = ReviewsClient.GetReviewsAsync(product.Id).Result;

				var extension = product.ToWebModel();
				var userHelper = new UserHelper();
				extension.User = userHelper.GetUser(product).Result;

				extension.Rating = reviews.TotalCount > 0 ? reviews.Items.Average(x => x.Rating) : 0;
				return PartialView("DisplayTemplates/Item", extension);
			}
			catch (Exception)
			{
				return null;
			}
		}
	}
}