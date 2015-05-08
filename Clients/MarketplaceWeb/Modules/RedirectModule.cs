using MarketplaceWeb.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.Extensions;
using VirtoCommerce.ApiClient.Utilities;

namespace MarketplaceWeb.Modules
{
	public class RedirectModule : IHttpModule
	{
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
				var category = Task.Run(() => SearchClient.GetCategoryByCodeAsync("MarketPlace", "en-US", id)).Result;
				if (category != null)
				{
					context.RewritePath(context.Request.Path.Replace("/" + id, string.Empty) + "/cat/" + id);
				}

				var product = Task.Run(() => SearchClient.GetProductByCodeAsync("MarketPlace", "en-US", id)).Result;
				if (product != null)
				{
					context.RewritePath(context.Request.Path.Replace("/" + id, string.Empty) + "/modules/" + id);
				}

				var vendor = Task.Run(() => CustomerServiceClient.GetContactByIdAsync(id)).Result;
				if (vendor != null)
				{
					context.RewritePath(context.Request.Path.Replace("/" + id, string.Empty) + "/vendor/" + id);
				}
			}
		}

		public void Dispose()
		{
			
		}
	}
}