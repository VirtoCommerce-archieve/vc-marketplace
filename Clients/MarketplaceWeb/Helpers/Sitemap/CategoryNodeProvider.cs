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
                return ClientContext.Clients.CreateBrowseClient(ConnectionHelper.ApiConnectionString("vc-commerce-api-mp"));
            }
        }

        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            var response = Task.Run(()=>SearchClient.GetCategoriesAsync()).Result;
            var order = 0;
            return response.Items.OrderBy(x => x.Name).Select(cat => new DynamicNode
            {
                Action = "CategorySearch",
                Title = cat.Name,
                Key = cat.Id,
                Order = order++,
                ParentKey = cat.ParentId,
                RouteValues = new Dictionary<string, object> { { "categoryId", cat.Id } }
            });
        }
    }
}