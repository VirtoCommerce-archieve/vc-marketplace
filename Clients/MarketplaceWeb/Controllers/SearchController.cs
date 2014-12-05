using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MarketplaceWeb.Converters;
using MarketplaceWeb.Helpers;
using MarketplaceWeb.Helpers.Marketing;
using MarketplaceWeb.Models;
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.DataContracts;
using VirtoCommerce.ApiClient.Extensions;

namespace MarketplaceWeb.Controllers
{
    [RoutePrefix("search")]
    public class SearchController : ControllerBase
    {
        public async Task<ActionResult> Index(BrowseQuery parameters)
        {
            ViewBag.Title = String.Format("Searching by '{0}'", parameters.Search);

            var query = new BrowseQuery
            {
                Search = parameters.Search
            };

            var results = await SearchClient.GetProductsAsync(query);
            var model = results.Items.Select(x => x.ToWebModel()).ToArray();

            return View(model);
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

        [Route("extensions/{categoryId}")]
        public async Task<ActionResult> CategorySearch(string categoryId, BrowseQuery parameters, string name = "Index", bool savePreferences = true)
        {
            var category = await SearchClient.GetCategoryAsync(categoryId);

            if (category != null)
            {
                ViewBag.Title = String.Format("Searching in '{0}'", category.Name);

                CustomerSession.Current.Tags.Add(ContextFieldConstants.CategoryId, categoryId);
                CustomerSession.Current.CategoryId = categoryId;

                if (savePreferences)
                {
                    RestoreSearchPreferences(parameters);
                }

                var ids = category.Parents != null ? category.Parents.Select(x => x.Key).ToList() : new List<string>();
                ids.Add(categoryId);
                parameters.Outline = string.Join("/", ids);

                var results = await SearchClient.GetProductsAsync(parameters);

                var model = results.Items.Select(x => x.ToWebModel()).ToArray();

                return View(name, model);
            }

            throw new HttpException(404, "Category not found");
        }

        #region Private Helpers

        private void RestoreSearchPreferences(BrowseQuery parameters)
        {
            var pageSize = parameters.Take;
            var sort = parameters.SortProperty;
            var sortOrder = parameters.SortDirection;

            if (!pageSize.HasValue)
            {
                int parsedSize;
                if (Int32.TryParse(CookieHelper.GetCookieValue("pagesizecookie"), out parsedSize))
                {
                    pageSize = parsedSize;
                }
            }
            else
            {
                CookieHelper.SetCookie("pagesizecookie", pageSize.Value.ToString(CultureInfo.InvariantCulture), DateTime.Now.AddMonths(1));
            }

            if (!pageSize.HasValue)
            {
                pageSize = BrowseQuery.DefaultPageSize;
            }

            parameters.Take = pageSize;

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

            parameters.SortProperty = sort;
            parameters.SortDirection = sortOrder;
        }

        #endregion
    }
}