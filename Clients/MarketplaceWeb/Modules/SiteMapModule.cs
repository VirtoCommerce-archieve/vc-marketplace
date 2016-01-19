using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Xml.Linq;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client;
using MarketplaceWeb.Helpers;
using MarketplaceWeb.Models;
using VirtoCommerce.Client.Model;

namespace MarketplaceWeb.Modules
{
	public class SiteMapModule : IHttpModule
	{
		private static XNamespace _xmlNamespace = "http://www.sitemaps.org/schemas/sitemap/0.9";
		private static string _path = HostingEnvironment.MapPath("~/");
		private static string _baseUrl = "http://virtocommerce.com/apps";

		private static string _xmlUrlsetTag = "urlset";
		private static string _urlTag = "url";

		private static string _categoriesSitemapFileName = "categories_sitemap.xml";
		private static string _modulesSitemapFileName = "modules_sitemap.xml";
		private static string _vendorsSitemapFileName = "vendors_sitemap.xml";

		private Object lockObject = new Object();

		private static string _vendorIdPropertyName = "vendorId";

        public const string StoreName = "Marketplace";
        public const string Locale = "en-US";

        private readonly HmacApiClient _apiClient = new HmacApiClient(ConfigurationManager.ConnectionStrings["VirtoCommerceBaseUrl"].ConnectionString, ConfigurationManager.AppSettings["vc-public-ApiAppId"], ConfigurationManager.AppSettings["vc-public-ApiSecretKey"]);


        public SearchModuleApi SearchClient
		{
			get
			{
				return new SearchModuleApi(new VirtoCommerce.Client.Client.Configuration(_apiClient));
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

        public VirtoCommerceStoreModuleWebModelStore Store
        {
            get
            {
                return StoreClient.StoreModuleGetStoreById(StoreName);
            }
        }

        public void Init(HttpApplication context)
		{
			context.BeginRequest += (new EventHandler(this.Application_BeginRequest));
		}

		private void Application_BeginRequest(Object source, EventArgs e)
		{
            var vendorList = CustomerServiceClient.CustomerModuleSearch(null, null, null, null).Members.Select(m => m.Id).ToList();

			lock (lockObject)
			{
				var fileChangeDate = File.GetLastWriteTimeUtc(Path.Combine(_path, _categoriesSitemapFileName));
				if (fileChangeDate.Day != DateTime.UtcNow.Day)
				{
					try
					{
						AddCategoriesToSitemap();
						AddProductsToSitemap(vendorList);
						AddVendorsToSitemap(vendorList);
					}
					catch
					{

					}

				}
			}
		}

		private void AddCategoriesToSitemap()
		{
			var categoriesSitemap = new XElement(_xmlNamespace + _xmlUrlsetTag);

            var result = SearchClient.SearchModuleSearch(criteriaCatalogId: Store.Catalog, criteriaResponseGroup: "WithCategories");

			foreach (var category in result.Categories)
			{
				categoriesSitemap.Add(BuildUrlElement(category.Code));
			}

			categoriesSitemap.Save(Path.Combine(_path, _categoriesSitemapFileName));
		}

		private void AddProductsToSitemap(List<string> vendorList)
		{
			var productsSitemap = new XElement(_xmlNamespace + _xmlUrlsetTag);

			var query = new BrowseQuery
			{
				Take = 1000,
                ItemResponseGroup = "ItemLarge"
			};

            var products = GetProducts(query);

			if (products.Length > 0)
			{
				foreach (var product in products)
				{
					if (product.Properties.Any(p => p.Name == _vendorIdPropertyName))
					{
						vendorList.Add((string)product.Properties.First(p => p.Name == _vendorIdPropertyName).Values.FirstOrDefault().Value);
					}

					if (product.SeoInfos.Any())
					{
						productsSitemap.Add(BuildUrlElement(product.SeoInfos.First().SemanticUrl));
					}
				}

				productsSitemap.Save(Path.Combine(_path, _modulesSitemapFileName));
			}
		}

		private void AddVendorsToSitemap(List<string> vendorList)
		{
			var vendorsSitemap = new XElement(_xmlNamespace + _xmlUrlsetTag);

			if (vendorList.Any())
			{
				foreach (var vendorId in vendorList.Distinct())
				{
					var vendor = CustomerServiceClient.CustomerModuleGetContactById(vendorId);

					if (vendor != null)
					{
						vendorsSitemap.Add(BuildUrlElement(vendor.Id));
					}
				}

				vendorsSitemap.Save(Path.Combine(_path, _vendorsSitemapFileName));
			}
		}

		private static XElement BuildUrlElement(string keyword)
		{
			var urlElement = new XElement(_xmlNamespace + _urlTag);

			urlElement.Add(new XElement(_xmlNamespace + "loc", string.Format("{0}/{1}", _baseUrl, keyword)));
			urlElement.Add(new XElement(_xmlNamespace + "changefreq", "daily"));
			urlElement.Add(new XElement(_xmlNamespace + "priority", "0.8"));
			urlElement.Add(new XElement(_xmlNamespace + "lastmod", DateTime.UtcNow.ToString("yyyy-MM-dd")));

			return urlElement;
		}

		public void Dispose()
		{

		}

        protected VirtoCommerceCatalogModuleWebModelProduct[] GetProducts(BrowseQuery query)
        {
            var result = SearchClient.SearchModuleSearch(
                criteriaStoreId: StoreName,
                criteriaResponseGroup: query.ItemResponseGroup,
                criteriaOutline: query.Outline,
                criteriaLanguageCode: Locale,
                criteriaSkip: query.Skip,
                criteriaTake: query.Take);

            return result.Products.ToArray();
        }
    }
}