using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MvcSiteMapProvider;
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.DataContracts;
using VirtoCommerce.ApiClient.Extensions;
using VirtoCommerce.ApiClient.Utilities;

namespace MarketplaceWeb.Helpers.Sitemap
{
    public class CategoryNodeProvider : DynamicNodeProviderBase
    {

        public BrowseClient SearchClient
        {
            get
            {
                return ClientContext.Clients.CreateBrowseClient();
            }
        }

        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            var response = Task.Run(()=>SearchClient.GetCategoriesAsync("MarketPlace", "en-US")).Result;
            var order = 0;
            return response.Items.OrderBy(x => x.Name).Select(cat => new DynamicNode
            {
                Action = "CategorySearch",
                Title = cat.Name,
                Key = cat.Code,
                Order = order++,
                ParentKey = (cat.Parents != null && cat.Parents.Length > 0) ? cat.Parents[0].Id : null,
				RouteValues = new Dictionary<string, object> { { "categoryId", cat.Code } }
            });
        }
    }
}