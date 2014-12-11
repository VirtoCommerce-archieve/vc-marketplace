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
using VirtoCommerce.ApiClient.DataContracts;
using VirtoCommerce.ApiClient.Extensions;

namespace MarketplaceWeb.Controllers
{
    [RoutePrefix("extensions")]
    public class SearchController : ControllerBase
    {
        [Route("search")]
        public async Task<ActionResult> Index(BrowseQuery parameters)
        {
            ViewBag.Title = String.Format("Searching by '{0}'", parameters.Search);


            var results = await SearchClient.GetProductsAsync(parameters);
            var retVal = CreateSearchResult(results, parameters);

            return View(retVal);
        }

        public async Task<ActionResult> Find(string term)
        {
            ViewBag.Title = String.Format("Searching by '{0}'", term);

            var query = new BrowseQuery
            {
                Take = 15,
                Search = term.EscapeSearchTerm()
            };
            var results = await SearchClient.GetProductsAsync(query);

            var data = from i in results.Items select new { url = Url.Action("DisplayItem","Extension", new { id = i.Id }), value = i.Name };
            return Json(data.ToArray(), JsonRequestBehavior.AllowGet);
        }

        [Route("{categoryId}")]
        public async Task<ActionResult> CategorySearch(string categoryId, BrowseQuery parameters, string name = "Index", bool savePreferences = true)
        {
            var category = await SearchClient.GetCategoryAsync(categoryId);

            if (category != null)
            {
                ViewBag.Title = String.Format(category.Name);

                CustomerSession.Current.Tags.Add(ContextFieldConstants.CategoryId, categoryId);
                CustomerSession.Current.CategoryId = categoryId;

                if (savePreferences)
                {
                    RestoreSearchPreferences(parameters);
                }

                parameters.Outline = category.Outline;

                var results = await SearchClient.GetProductsAsync(parameters);

                var retVal = CreateSearchResult(results, parameters);

                return View(name, retVal);
            }

            throw new HttpException(404, "Category not found");
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
                var category =Task.Run(() => SearchClient.GetCategoryByCodeAsync(categoryUrl.CategoryCode)).Result;

                if (category != null)
                {
                    query.Outline = category.Outline;
                }
            }
            //Need to run synchrously because of child action
            var results = Task.Run(() => SearchClient.GetProductsAsync(query)).Result;
            var items = results.Items.Select(x => x.ToWebModel()).ToArray();
             
            return PartialView(items);
        }

        #region Private Helpers

        private SearchResult CreateSearchResult(ResponseCollection<Product> results, BrowseQuery query)
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