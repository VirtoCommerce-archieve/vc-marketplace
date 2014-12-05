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
                return ClientContext.Clients.CreateBrowseClient(ConnectionHelper.ApiConnectionString("vc-commerce-api"));
            }
        }
    
    }
}