using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarketplaceWeb.Helpers
{
	public static class SiteUrlHelper
	{
		public static string ResolveServerUrl(string serverRelativeUrl, bool forceHttps = false)
		{
			if (serverRelativeUrl.IndexOf("://") > -1)
				return serverRelativeUrl;

			if (!string.IsNullOrEmpty(serverRelativeUrl))
				serverRelativeUrl = VirtualPathUtility.ToAbsolute(serverRelativeUrl);
			else
				serverRelativeUrl = VirtualPathUtility.ToAbsolute("~").TrimEnd('/');

			string newUrl = serverRelativeUrl;
			Uri originalUri = System.Web.HttpContext.Current.Request.Url;
			newUrl = (forceHttps ? "https" : originalUri.Scheme) +
				"://" + originalUri.Authority + newUrl;
			return newUrl;
		}

	}
}