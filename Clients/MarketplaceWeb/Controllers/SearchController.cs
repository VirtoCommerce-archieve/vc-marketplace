using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.ApiClient.Extensions;

namespace MarketplaceWeb.Controllers
{


    [RoutePrefix("search")]
    public class SearchController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Find(string term)
        {
            ViewBag.Title = String.Format("Searching by '{0}'", term);

            //var parameters = new SearchParameters { PageSize = 15 };
            //var criteria = new CatalogItemSearchCriteria { SearchPhrase = term.EscapeSearchTerm(), IsFuzzySearch = true };
            //var results = SearchResults(criteria, parameters);

            //var data = from i in results.CatalogItems
            //           select new { url = Url.ItemUrl(i.CatalogItem.Item, i.CatalogItem.ParentItemId), value = i.DisplayName };

            var results = new[] { 0, 1, 2 };

            var data = from i in results select new { url = "http://extensions/"+i, value = "Extension " + i };
            return Json(data.ToArray(), JsonRequestBehavior.AllowGet);
        }
    }
}