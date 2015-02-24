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

namespace MarketplaceWeb.Controllers
{
	[RoutePrefix("extension")]
	public class ExtensionController : ControllerBase
	{

		[Route("{id}")]
		public async Task<ActionResult> DisplayItem(string id)
		{
			var item = await SearchClient.GetProductAsync(id);
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

			if (item.SeoKeywords.Any())
			{
				ViewBag.Title = item.SeoKeywords[0].Title;
				ViewBag.Description = item.SeoKeywords[0].MetaDescription;
			}

			return View(model);
		}

		/// <summary>
		/// This method used only for dynamic content. It cannot be exectuted async due to request limitations
		/// </summary>
		[ChildActionOnly]
		public ActionResult DisplayDynamic(string itemCode)
		{
			try
			{
				var product = Task.Run(() => SearchClient.GetProductByCodeAsync(itemCode)).Result;
				var reviews = Task.Run(() => ReviewsClient.GetReviewsAsync(product.Id)).Result;

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