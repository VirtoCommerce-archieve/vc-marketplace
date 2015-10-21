using MarketplaceWeb.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client;
using System.Configuration;

namespace MarketplaceWeb.Modules
{
	public class RedirectModule : IHttpModule
	{
        public const string StoreName = "Marketplace";
        public const string Locale = "en-US";

        private readonly HmacApiClient _apiClient = new HmacApiClient(ConfigurationManager.ConnectionStrings["VirtoCommerceBaseUrl"].ConnectionString, ConfigurationManager.AppSettings["vc-public-ApiAppId"], ConfigurationManager.AppSettings["vc-public-ApiSecretKey"]);

        public ApiHelper _apiHelper = new ApiHelper();

        public CommerceCoreModuleApi CommerceClient
        {
            get
            {
                return new CommerceCoreModuleApi(_apiClient);
            }
        }

        public CatalogModuleApi CatalogClient
        {
            get
            {
                return new CatalogModuleApi(_apiClient);
            }
        }

        public MerchandisingModuleApi MerchandisingClient
        {
            get
            {
                return new MerchandisingModuleApi(_apiClient);
            }
        }

		public CustomerManagementModuleApi CustomerServiceClient
		{
			get
			{
                return new CustomerManagementModuleApi(_apiClient);
			}
		}

		public void Init(HttpApplication context)
		{
			context.BeginRequest += (new EventHandler(this.Application_BeginRequest));
		}

		private void Application_BeginRequest(Object source, EventArgs e)
		{
			// Create HttpApplication and HttpContext objects to access
			// request and response properties.
			HttpApplication application = (HttpApplication)source;
			HttpContext context = application.Context;
			string filePath = context.Request.Path;

			if (filePath.StartsWith("/vccom/"))
			{
				filePath = filePath.Replace("/vccom/", string.Empty);
			}

			var steps = filePath.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
			if (steps.Length > 0)
			{
				var id = steps.Last();
				bool stop = false;

                var category = _apiHelper.GetCategory(MerchandisingClient, StoreName, Locale, id);
                if (category != null)
				{
					context.RewritePath(context.Request.Path.Replace("/" + id, string.Empty) + "/cat/" + id);
					stop = true;
				}

				if (!stop)
				{
					var product = _apiHelper.GetProduct(CommerceClient, CatalogClient, id);
					if (product != null)
					{
						context.RewritePath(context.Request.Path.Replace("/" + id, string.Empty) + "/modules/" + id);
						stop = true;
					}

					if (!stop)
					{
						var vendor = _apiHelper.GetContact(CustomerServiceClient, id);
						if (vendor != null)
						{
							context.RewritePath(context.Request.Path.Replace("/" + id, string.Empty) + "/vendor/" + id);
						}
					}
				}
			}
		}

		public void Dispose()
		{
			
		}
	}
}