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
        public ActionResult ShowDynamicContent(string placeName)
        {
            var items = Task.Run(()=>ContentClient.GetDynamicContentAsync(placeName, CustomerSession.Current.Tags)).Result;
            if (items != null && items.TotalCount > 0)
            {
                return PartialView("BaseContentPlace", new BannerModel(items.Items.ToArray()));
            }
            return null;
        }

        //[DonutOutputCache(CacheProfile = "BannerCache")]
        public ActionResult ShowDynamicContents(string[] placeName)
        {
            return PartialView("MultiBanner", placeName);
        }
    }
}