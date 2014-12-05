using System;
using System.Net.Http;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient.DataContracts;
using VirtoCommerce.ApiClient.Utilities;

namespace VirtoCommerce.ApiClient
{
    public class BrowseClient : BaseClient
    {
        protected class RelativePaths
        {
            public const string Products = "products";
            public const string Categories = "categories";
            public const string Category = "categories/{0}";
            public const string Product = "products/{0}";
        }

        /// <summary>
        /// Initializes a new instance of the AdminManagementClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
        /// <param name="token">Access token</param>
        public BrowseClient(Uri adminBaseEndpoint, string token)
            : base(adminBaseEndpoint, new TokenMessageProcessingHandler(token))
        {
        }

        /// <summary>
        /// Initializes a new instance of the AdminManagementClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
        /// <param name="handler"></param>
        public BrowseClient(Uri adminBaseEndpoint, MessageProcessingHandler handler)
            : base(adminBaseEndpoint, handler)
        {

        }

        /// <summary>
        /// List items matching the given query
        /// </summary>
        public Task<ResponseCollection<Product>> GetProductsAsync(BrowseQuery query)
        {
            return GetAsync<ResponseCollection<Product>>(CreateRequestUri(RelativePaths.Products, query.GetQueryString()));
        }

        public Task<Product> GetProductAsync(string productId)
        {
            return GetAsync<Product>(CreateRequestUri(String.Format(RelativePaths.Product, productId)));
        }

        public Task<ResponseCollection<Category>> GetCategoriesAsync(string parentId = null)
        {
            return GetAsync<ResponseCollection<Category>>(CreateRequestUri(RelativePaths.Categories, "parentId=" + parentId));
        }


        public Task<Category> GetCategoryAsync(string categoryId)
        {
            return GetAsync<Category>(CreateRequestUri(String.Format(RelativePaths.Category, categoryId)));
        }
    }
}
