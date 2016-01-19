using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using MarketplaceWeb.Helpers.Marketing;
using MarketplaceWeb.Models;
using System.Threading.Tasks;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client;
using VirtoCommerce.Client.Model;
using MarketplaceWeb.Helpers;

namespace MarketplaceWeb.Controllers
{
	public abstract class ControllerBase : Controller
	{
		public const string StoreName = "Marketplace";
		public const string Locale = "en-US";

        private readonly HmacApiClient _apiClient = new HmacApiClient(ConfigurationManager.ConnectionStrings["VirtoCommerceBaseUrl"].ConnectionString, ConfigurationManager.AppSettings["vc-public-ApiAppId"], ConfigurationManager.AppSettings["vc-public-ApiSecretKey"]);

        public ApiHelper ApiHelper = new ApiHelper();

        public ControllerBase()
        {
            this.Store = StoreClient.StoreModuleGetStoreById(StoreName);
        }

		public SearchModuleApi SearchClient
		{
			get
			{
				return new SearchModuleApi(new VirtoCommerce.Client.Client.Configuration(_apiClient));
			}
		}

        public CatalogModuleApi CatalogClient
        {
            get
            {
                return new CatalogModuleApi(new VirtoCommerce.Client.Client.Configuration(_apiClient));
            }
        }

        public CustomerManagementModuleApi CustomerServiceClient
        {
            get
            {
                return new CustomerManagementModuleApi(new VirtoCommerce.Client.Client.Configuration(_apiClient));
            }
        }

        public StoreModuleApi StoreClient
        {
            get
            {
                return new StoreModuleApi(new VirtoCommerce.Client.Client.Configuration(_apiClient));
            }
        }

        public CommerceCoreModuleApi CommerceClient
        {
            get
            {
                return new CommerceCoreModuleApi(new VirtoCommerce.Client.Client.Configuration(_apiClient));
            }
        }

        protected VirtoCommerceCatalogModuleWebModelProduct[] GetProducts(BrowseQuery query)
        {
            var result = SearchClient.SearchModuleSearch(
                criteriaStoreId: StoreName,
                criteriaCatalogId: Store.Catalog,
                criteriaResponseGroup: "21",
                criteriaOutline: query.Outline,
                criteriaCategoryId: query.CategoryId,
                criteriaSkip: query.Skip,
                criteriaTake: query.Take);

            return result.Products.ToArray();
        }

        public VirtoCommerceStoreModuleWebModelStore Store
        {
            get; set;
        }
    }
}