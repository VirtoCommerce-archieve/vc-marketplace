using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;

namespace MarketplaceWeb.Models
{
    public class BrowseQuery
    {
        public BrowseQuery()
        {
            Filters = new Dictionary<string, string[]>();
        }

        /// <summary>
        ///     The default page size
        /// </summary>
        public const int DefaultPageSize = 8;

        public Dictionary<string, string[]> Filters { get; set; }

        /// <summary>
        ///     Gets or sets the outline to filter products. Its category path.
        /// </summary>
        [DefaultValue("")]
        public string Outline { get; set; }

        public string[] PriceLists { get; set; }

        /// <summary>
        ///     Gets or sets the string on which to filter as part of this query
        /// </summary>
        [DefaultValue("")]
        public string Search { get; set; }

        /// <summary>
        ///     Gets or sets the number of items to skip as part of this query
        /// </summary>
        public int? Skip { get; set; }

        /// <summary>
        ///     Gets or sets the direction on which to sort on as part of this query
        /// </summary>
        public string SortDirection { get; set; }

        /// <summary>
        ///     Gets or sets the property to sort on as part of this query
        /// </summary>
        public string SortProperty { get; set; }

        /// <summary>
        ///     Gets or sets the start date from.
        /// </summary>
        public DateTime? StartDateFrom { get; set; }

        /// <summary>
        ///     Gets or sets the number of items to take as part of this query
        /// </summary>
        [DefaultValue(20)]
        public int? Take { get; set; }

        public string ItemResponseGroup { get; set; }

        public string StoreName { get; set; }

        public string Locale { get; set; }
    }
}