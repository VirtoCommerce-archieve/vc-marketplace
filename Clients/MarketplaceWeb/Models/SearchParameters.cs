using System.Collections.Generic;
using System.Text;

namespace MarketplaceWeb.Models
{
    public class SearchParameters
    {
        /// <summary>
        /// The default page size
        /// </summary>
        public const int DefaultPageSize = 8;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchParameters"/> class.
        /// </summary>
        public SearchParameters()
        {
            Facets = new Dictionary<string, string[]>();
            PageSize = 0;
            PageIndex = 1;
        }

        /// <summary>
        /// Gets or sets the free search.
        /// </summary>
        /// <value>The free search.</value>
        public string FreeSearch { get; set; }
        /// <summary>
        /// Gets or sets the index of the page.
        /// </summary>
        /// <value>The index of the page.</value>
        public int PageIndex { get; set; }
        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>The size of the page.</value>
        public int PageSize { get; set; }
        /// <summary>
        /// Gets or sets the facets.
        /// </summary>
        /// <value>The facets.</value>
        public IDictionary<string, string[]> Facets { get; set; }
        /// <summary>
        /// Gets or sets the sort.
        /// </summary>
        /// <value>The sort.</value>
        public string Sort { get; set; }

        /// <summary>
        /// Gets or sets the sort order asc or desc.
        /// </summary>
        /// <value>True if descending, false if ascending</value>
        public string SortOrder { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(FreeSearch);
            builder.Append(PageIndex);
            builder.Append(PageSize);
            builder.Append(Sort);

            foreach (var facet in Facets)
            {
                builder.Append(facet);
            }

            return builder.ToString();
        }
    }
}