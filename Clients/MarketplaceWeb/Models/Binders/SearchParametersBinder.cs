﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using VirtoCommerce.ApiClient.Extensions;

namespace MarketplaceWeb.Models.Binders
{
    public class SearchParametersBinder : IModelBinder
    {
        /// <summary>
        /// Name values to dictionary.
        /// </summary>
        /// <param name="nv">The nv.</param>
        /// <returns>IDictionary{System.StringSystem.String}.</returns>
        public IDictionary<string, string> NvToDict(NameValueCollection nv)
        {
            var d = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            foreach (var k in nv.AllKeys)
                d[k] = nv[k];
            return d;
        }

        /// <summary>
        /// The facet regex
        /// </summary>
        private static readonly Regex FacetRegex = new Regex("^f_", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        protected virtual NameValueCollection GetParams(ControllerContext controllerContext)
        {
            return controllerContext.HttpContext.Request.QueryString;
        }

        /// <summary>
        /// Binds the model to a value by using the specified controller context and binding context.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="bindingContext">The binding context.</param>
        /// <returns>The bound value.</returns>
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {

            var parameters = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var sp = parameters != null ? parameters.RawValue as SearchParameters : null;
            if (sp == null)
            {
                var qs = GetParams(controllerContext);
                var qsDict = NvToDict(qs);
                var facets = qsDict.Where(k => FacetRegex.IsMatch(k.Key)).Select(k => k.WithKey(FacetRegex.Replace(k.Key, ""))).ToDictionary(x => x.Key, y => y.Value.Split(','));

                sp = new SearchParameters
                {
                    FreeSearch = qs["q"].EmptyToNull(),
                    PageIndex = qs["p"].TryParse(1),
                    PageSize = qs["pageSize"].TryParse(0),
                    Sort = qs["sort"].EmptyToNull(),
                    SortOrder = qs["sortorder"].EmptyToNull(),
                    Facets = facets
                };
                if (!string.IsNullOrEmpty(sp.FreeSearch))
                {
                    sp.FreeSearch = sp.FreeSearch.EscapeSearchTerm();
                }
            }
            return sp;
        }
    }
}