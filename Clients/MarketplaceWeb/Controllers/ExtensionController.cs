using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MarketplaceWeb.Converters;
using VirtoCommerce.ApiClient;

namespace MarketplaceWeb.Controllers
{
    [RoutePrefix("extension")]
    public class ExtensionController : ControllerBase
    {

        [Route("{id}")]
        public async Task<ActionResult> Display(string id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method used only for dynamic content. It cannot be exectuted async due to request limitations
        /// </summary>
        [ChildActionOnly]
        public ActionResult DisplayDynamic(string itemCode)
        {
            //TODO here should call get by code when available
            var result = Task.Run(() => SearchClient.GetProductAsync(itemCode)).Result;
            var model = result.ToWebModel();
            return PartialView("DisplayTemplates/Item", model);
        }
    }
}