using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Xml.Linq;
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.Extensions;
using VirtoCommerce.ApiClient.DataContracts;

namespace MarketplaceWeb.Modules
{
	public class SiteMapModule : IHttpModule
	{
		private static string _storeName = ConfigurationManager.AppSettings["StoreName"];
		private static string _storeLocalization = ConfigurationManager.AppSettings["StoreLocalization"];

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

		public BrowseClient SearchClient
		{
			get
			{
				return ClientContext.Clients.CreateBrowseClient();
			}
		}

		public CustomerServiceClient CustomerServiceClient
		{
			get
			{
				return ClientContext.Clients.CreateCustomerServiceClient();
			}
		}

		public void Init(HttpApplication context)
		{
			context.BeginRequest += (new EventHandler(this.Application_BeginRequest));
		}

		private void Application_BeginRequest(Object source, EventArgs e)
		{
			var vendorList = new List<string>();

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

			var categories = Task.Run(() => SearchClient.GetCategoriesAsync(_storeName, _storeLocalization)).Result;

			if (categories.TotalCount > 0)
			{
				foreach (var category in categories.Items)
				{
					categoriesSitemap.Add(BuildUrlElement(category.Code));
				}

				categoriesSitemap.Save(Path.Combine(_path, _categoriesSitemapFileName));
			}
		}

		private void AddProductsToSitemap(List<string> vendorList)
		{
			var productsSitemap = new XElement(_xmlNamespace + _xmlUrlsetTag);

			var query = new BrowseQuery
			{
				Search = string.Empty.EscapeSearchTerm(),
				Take = 1000
			};

			var products = Task.Run(() => SearchClient.GetProductsAsync(_storeName, _storeLocalization, query, ItemResponseGroups.ItemLarge)).Result;

			if (products.TotalCount > 0)
			{
				foreach (var product in products.Items)
				{
					if (product.Properties.Any(p => p.Key == _vendorIdPropertyName))
					{
						vendorList.Add((string)product.Properties.First(p => p.Key == _vendorIdPropertyName).Value);
					}

					if (product.Seo.Any())
					{
						productsSitemap.Add(BuildUrlElement(product.Seo.First().Keyword));
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
					var vendor = Task.Run(() => CustomerServiceClient.GetContactByIdAsync(vendorId)).Result;

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
	}
}