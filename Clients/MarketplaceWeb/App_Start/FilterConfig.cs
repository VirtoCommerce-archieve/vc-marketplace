using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.Extensions;

namespace MarketplaceWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleAndLogErrorAttribute());
        }
    }

    public class HandleAndLogErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            if (filterContext.IsChildAction)
            {
                return;
            }

            // If custom errors are disabled, we need to let the normal ASP.NET exception handler
            // execute so that the user can see useful debugging information.
            if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
            {
                return;
            }

            var exception = filterContext.Exception;

            var statusCode = (int)HttpStatusCode.InternalServerError;
            var errorPage = "";

            var httpException = filterContext.Exception as HttpException;
            if (httpException != null)
            {
                statusCode = httpException.GetHttpCode();
            }
            else if (exception.Is<UnauthorizedAccessException>())
            {
                //to prevent login prompt in IIS
                // which will appear when returning 401.
                statusCode = (int)HttpStatusCode.Forbidden;
            }

            if (exception.Is<ManagementClientException>())
            {
                var clientEx = exception as ManagementClientException;

                //Skip NotFound from api service
                if (clientEx.StatusCode == HttpStatusCode.NotFound)
                {
                    filterContext.ExceptionHandled = true;
                    return;
                }
                errorPage = "~/Views/Error/ServiceError.cshtml";
            }

            var result = CreateActionResult(filterContext, statusCode, errorPage);

            filterContext.Result = result;

            //// Prepare the response code.
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = statusCode;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }

        /// <summary>
        /// Creates the action result.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="errorPage">The error page.</param>
        /// <returns>ActionResult.</returns>
        protected virtual ActionResult CreateActionResult(ExceptionContext filterContext, int statusCode,
                                                          string errorPage = "")
        {
            var ctx = new ControllerContext(filterContext.RequestContext, filterContext.Controller);

            var specificPage = errorPage;

            if (string.IsNullOrEmpty(specificPage))
            {
                switch (statusCode)
                {
                    case 403:
                        specificPage = "~/Views/Error/Forbidden.cshtml";
                        break;
                    case 404:
                        specificPage = "~/Views/Error/Oops.cshtml";
                        break;
                }
            }


            var viewName = SelectFirstView(ctx, specificPage, string.Format("~/Views/Error/{0}.cshtml", statusCode),
                                           "~/Views/Error/Oops.cshtml", "Error");

            var controllerName = (string)filterContext.RouteData.Values["controller"];
            var actionName = (string)filterContext.RouteData.Values["action"];
            var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
            var result = new ViewResult
            {
                ViewName = viewName,
                MasterName = "~/Views/Shared/_ErrorLayout.cshtml",
                ViewData = new ViewDataDictionary<HandleErrorInfo>(model)
            };
            result.ViewBag.StatusCode = statusCode;
            return result;
        }

        /// <summary>
        /// Selects the first view.
        /// </summary>
        /// <param name="ctx">The ControllerContext.</param>
        /// <param name="viewNames">The view names.</param>
        /// <returns>System.String.</returns>
        protected string SelectFirstView(ControllerContext ctx, params string[] viewNames)
        {
            return viewNames.First(view => !string.IsNullOrWhiteSpace(view) && ViewExists(ctx, view));
        }

        /// <summary>
        /// Views the exists.
        /// </summary>
        /// <param name="ctx">The CTX.</param>
        /// <param name="name">The name.</param>
        /// <returns><c>true</c> if view exist, <c>false</c> otherwise.</returns>
        protected bool ViewExists(ControllerContext ctx, string name)
        {
            var result = ViewEngines.Engines.FindView(ctx, name, null);
            return result.View != null;
        }
    }
}
