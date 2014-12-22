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

            var model = new FullExtension
            {
                Extension = item.ToWebModel()
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
                var model = product.ToWebModel();
                model.Rating = reviews.TotalCount > 0 ? reviews.Items.Average(x => x.Rating) : 0;
                return PartialView("DisplayTemplates/Item", model);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}