using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using MarketplaceWeb.Helpers.Marketing;
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.Extensions;
using VirtoCommerce.ApiClient.Utilities;
using MarketplaceWeb.Models;
using System.Threading.Tasks;

namespace MarketplaceWeb.Controllers
{
	public abstract class ControllerBase : Controller
	{
		public BrowseClient SearchClient
		{
			get
			{
				return ClientContext.Clients.CreateBrowseClient();
			}
		}

		//public  ContentClient
		//{
		//	get
		//	{
		//		return ClientContext.Clients.CreateDefaultContentClient();
		//	}
		//}

		public ReviewsClient ReviewsClient
		{
			get
			{
				return ClientContext.Clients.CreateReviewsClient();
			}
		}

		public SecurityClient SecurityClient
		{
			get
			{
				return ClientContext.Clients.CreateSecurityClient();
			}
		}

		public CustomerServiceClient CustomerServiceClient
		{
			get
			{
				return ClientContext.Clients.CreateCustomerServiceClient();
			}
		}

		public StoreClient StoreClient
		{
			get
			{
				return ClientContext.Clients.CreateStoreClient();
			}
		}
	}
}