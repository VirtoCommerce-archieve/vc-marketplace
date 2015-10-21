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

		public MerchandisingModuleApi MerchandisingClient
		{
			get
			{
				return new MerchandisingModuleApi(_apiClient);
			}
		}

        public CatalogModuleApi CatalogClient
        {
            get
            {
                return new CatalogModuleApi(_apiClient);
            }
        }

        public CustomerManagementModuleApi CustomerServiceClient
        {
            get
            {
                return new CustomerManagementModuleApi(_apiClient);
            }
        }

        public StoreModuleApi StoreClient
        {
            get
            {
                return new StoreModuleApi(_apiClient);
            }
        }

        public CommerceCoreModuleApi CommerceClient
        {
            get
            {
                return new CommerceCoreModuleApi(_apiClient);
            }
        }

        protected VirtoCommerceMerchandisingModuleWebModelProduct[] GetProducts(BrowseQuery query)
        {
            var result = MerchandisingClient.MerchandisingModuleProductSearch(
                StoreName,
                null,
                query.ItemResponseGroup,
                query.Outline,
                Locale,
                null,
                null,
                null,
                null,
                null,
                query.Skip,
                query.Take,
                null,
                null);

            return result.Items.ToArray();
        }
    }
}