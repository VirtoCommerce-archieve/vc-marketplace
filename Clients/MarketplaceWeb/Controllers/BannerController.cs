using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MarketplaceWeb.Converters;
using MarketplaceWeb.Helpers.Marketing;
using MarketplaceWeb.Models;
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.DataContracts;

namespace MarketplaceWeb.Controllers
{
    public class BannerController : ControllerBase
    {
     
        /// <summary>
        /// Shows the dynamic content
        /// </summary>
        /// <param name="placeName">Name of dynamic content place.</param>
        /// <returns>ActionResult.</returns>
        //[DonutOutputCache(CacheProfile = "BannerCache")]
        public async Task<ActionResult> ShowDynamicContent(string placeName)
        {
            var result = await ContentClient.GetDynamicContentAsync(new[] { placeName }, CustomerSession.Current.Tags);
            if (result != null && result.TotalCount > 0)
            {
                return PartialView("BaseContentPlace", new BannerModel(result.Items.First().Items.ToArray()));
            }
            return null;
        }

        //[DonutOutputCache(CacheProfile = "BannerCache")]
        public async Task<ActionResult> ShowDynamicContents(string[] placeName)
        {
            var result = await ContentClient.GetDynamicContentAsync(placeName, CustomerSession.Current.Tags);
            if (result != null && result.TotalCount > 0)
            {
                return PartialView("MultiBanner", result.Items);
            }
            return null;
        }
    }
}