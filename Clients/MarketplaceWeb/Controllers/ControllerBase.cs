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

namespace MarketplaceWeb.Controllers
{
    public abstract class ControllerBase : Controller
	{
        public BrowseClient SearchClient
        {
            get
            {
                return ClientContext.Clients.CreateBrowseClient(ConnectionHelper.ApiConnectionString("vc-commerce-api-mp"));
            }
        }

        public ContentClient ContentClient
        {
            get
            {
                return ClientContext.Clients.CreateContentClient(ConnectionHelper.ApiConnectionString("vc-commerce-api-mp", null));
            }
        }

        public ReviewsClient ReviewsClient
        {
            get
            {
                return ClientContext.Clients.CreateReviewsClient(ConnectionHelper.ApiConnectionString("vc-commerce-api-mp"));
            }
        }

        public SecurityClient SecurityClient
        {
            get
            {
                return ClientContext.Clients.CreateSecurityClient();
            }
        }
    
    }
}