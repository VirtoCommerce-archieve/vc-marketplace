using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MarketplaceWeb.Helpers;
using MarketplaceWeb.Models;
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.DataContracts;
using VirtoCommerce.ApiClient.Extensions;

namespace MarketplaceWeb.Controllers
{
    [RoutePrefix("search")]
    public class SearchController : ControllerBase
    {
        public async Task<ActionResult> Index(SearchParameters parameters)
        {
            ViewBag.Title = String.Format("Searching by '{0}'", parameters.FreeSearch);

            var query = new BrowseQuery
            {
                Search = parameters.FreeSearch
            };

            var results = await SearchClient.GetProductsAsync(query);

            return View(results);
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

        #region Private Helpers

        private void RestoreSearchPreferences(SearchParameters parameters)
        {
            var pageSize = parameters.PageSize;
            var sort = parameters.Sort;
            var sortOrder = parameters.SortOrder;

            if (pageSize == 0)
            {
                Int32.TryParse(CookieHelper.GetCookieValue("pagesizecookie"), out pageSize);
            }
            else
            {
                CookieHelper.SetCookie(
                    "pagesizecookie", pageSize.ToString(CultureInfo.InvariantCulture), DateTime.Now.AddMonths(1));
            }

            if (pageSize == 0)
            {
                pageSize = SearchParameters.DefaultPageSize;
            }

            parameters.PageSize = pageSize;

            if (String.IsNullOrEmpty(sort))
            {
                sort = CookieHelper.GetCookieValue("sortcookie");
            }
            else
            {
                CookieHelper.SetCookie("sortcookie", sort, DateTime.Now.AddMonths(1));
            }

            if (String.IsNullOrEmpty(sortOrder))
            {
                sortOrder = CookieHelper.GetCookieValue("sortordercookie");
            }
            else
            {
                CookieHelper.SetCookie("sortordercookie", sortOrder, DateTime.Now.AddMonths(1));
            }

            parameters.Sort = sort;
            parameters.SortOrder = sortOrder;
        }

        #endregion
    }
}