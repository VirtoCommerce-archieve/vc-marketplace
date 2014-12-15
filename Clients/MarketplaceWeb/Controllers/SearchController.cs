using System.Diagnostics;
using System.Net;
using MarketplaceWeb.Converters;
using MarketplaceWeb.Helpers;
using MarketplaceWeb.Helpers.Marketing;
using MarketplaceWeb.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.DataContracts;
using VirtoCommerce.ApiClient.Extensions;

namespace MarketplaceWeb.Controllers
{
    [RoutePrefix("extensions")]
    public class SearchController : ControllerBase
    {
        [Route("search")]
        public async Task<ActionResult> Index(BrowseQuery query)
        {

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                ViewBag.Title = String.Format("Searching by '{0}'", query.Search);
            }

            var retVal = await SearchAsync(query);

            return View(retVal);
        }

        [Route("{categoryId}")]
        public async Task<ActionResult> CategorySearch(string categoryId, BrowseQuery query)
        {
            var category = await SearchClient.GetCategoryAsync(categoryId);

            if (category != null)
            {
                ViewBag.Title = String.Format(category.Name);

                CustomerSession.Current.Tags.Add(ContextFieldConstants.CategoryId, categoryId);
                CustomerSession.Current.CategoryId = categoryId;

                RestoreSearchPreferences(query);

                query.Outline = category.Outline;

                var retVal = await SearchAsync(query);

                return View("Index", retVal);
            }

            throw new HttpException(404, "Category not found");
        }

        [Route("developer/{userId}")]
        public async Task<ActionResult> DevelopersExtensions(BrowseQuery query, string userId)
        {
            ViewBag.Title = "Developer extensions";
            query.Filters.Add("userId", new[] { userId });
            var retVal = await SearchAsync(query);
            return View("Index", retVal);
        }

        public async Task<ActionResult> Find(string term)
        {
            ViewBag.Title = String.Format("Searching by '{0}'", term);

            var query = new BrowseQuery
            {
                Take = 15, //autocomplete returns first 15
                Search = term.EscapeSearchTerm()
            };
            var results = await SearchClient.GetProductsAsync(query);

            var data = from i in results.Items
                       select new
                           {
                               url = Url.Action("DisplayItem", "Extension", new { id = i.Id }),
                               value = i.Name
                           };
            return Json(data.ToArray(), JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        public ActionResult SearchItems(CategoryUrlModel categoryUrl)
        {
            var query = new BrowseQuery
            {
                SortProperty = categoryUrl.SortField,
                Take = categoryUrl.ItemCount
            };

            if (categoryUrl.NewItemsOnly)
            {
                query.StartDateFrom = DateTime.UtcNow.AddMonths(-1);
            }

            if (!string.IsNullOrWhiteSpace(categoryUrl.CategoryCode))
            {
                var category = Task.Run(() => SearchClient.GetCategoryByCodeAsync(categoryUrl.CategoryCode)).Result;

                if (category != null)
                {
                    query.Outline = category.Outline;
                }
            }

            //Need to run synchrously because of child action
            var model = Search(query);

            return PartialView(model.Results);
        }

        #region Private Helpers

        private static SearchResult CreateSearchResult(ResponseCollection<Product> results, BrowseQuery query)
        {
            var retVal = new SearchResult
            {
                Results = results.Items.Select(x => x.ToWebModel()).ToList(),
                Pager =
                {
                    TotalCount = results.TotalCount,
                    CurrentPage = query.Skip ?? 0,
                    RecordsPerPage = query.Take ?? BrowseQuery.DefaultPageSize,
                    StartingRecord = query.Take ?? 0 * query.Skip ?? 0,
                    DisplayStartingRecord = query.Skip ?? 0 + 1,
                    SortValues = new[] { "Price", "Rating" },
                    SelectedSort = query.SortProperty,
                    SortOrder = query.SortDirection
                }
            };

            var end = query.Skip + query.Take ?? 0;
            retVal.Pager.DisplayEndingRecord = end > results.TotalCount ? results.TotalCount : end;

            return retVal;
        }

        private async Task<SearchResult> SearchAsync(BrowseQuery query)
        {
            var results = await SearchClient.GetProductsAsync(query);
            var retVal = CreateSearchResult(results, query);

            return retVal;
        }

        private SearchResult Search(BrowseQuery query)
        {
            return Task.Run(() => SearchAsync(query)).Result;
        }

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