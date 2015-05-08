using MarketplaceWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MarketplaceWeb.Helpers
{
	public static class HtmlHelperExtensions
	{
		public static string Compatibility(this HtmlHelper helper, Module module)
		{
			return string.Join(", ", module.Compatibility);
		}

		public static string Locale(this HtmlHelper helper, Module module)
		{
			return string.Join(", ", module.Locale);
		}

		public static string LastRelease(this HtmlHelper helper, Module module)
		{
			if (module.LatestRelease != null)
			{
				return string.Format("{0} from {1}",
					module.LatestRelease.Version,
					module.LatestRelease.ReleaseDate.Value.ToString("MM/dd/yyyy"));
			}

			return string.Empty;
		}

		public static MvcHtmlString ShareLinksBlock(this HtmlHelper helper, Module module)
		{
			var socialNetworks = ConfigurationManager.AppSettings["SocialNetworks"];

			var retVal = new StringBuilder();

			if(socialNetworks.Contains("tw"))
			{
				retVal.AppendLine("<li class=\"list-item __tw\">");

				retVal.AppendLine(
					string.Format(
					"<a target=\"_blank\" href=\"http://www.twitter.com/share?url={0}\" class=\"list-link\">Twitter</a>",
					HttpUtility.UrlEncode(string.Format(SiteUrlHelper.ResolveServerUrl("~/modules/{0}"), module.Code))));

				retVal.AppendLine("</li>");
			}
			if(socialNetworks.Contains("fb"))
			{
				retVal.AppendLine("<li class=\"list-item __fb\">");

				retVal.AppendLine(
					string.Format(
					"<a target=\"_blank\" href=\"https://www.facebook.com/sharer/sharer.php?u={0}\" class=\"list-link\">Facebook</a>",
					HttpUtility.UrlEncode(string.Format(SiteUrlHelper.ResolveServerUrl("~/modules/{0}"), module.Code))));
				
				retVal.AppendLine("</li>");
			}
			if(socialNetworks.Contains("gp"))
			{
				retVal.AppendLine("<li class=\"list-item __gp\">");

				retVal.AppendLine(
					string.Format(
					"<a target=\"_blank\" href=\"https://plus.google.com/share?url={0}\" class=\"list-link\">Google plus</a>",
					HttpUtility.UrlEncode(string.Format(SiteUrlHelper.ResolveServerUrl("~/modules/{0}"), module.Code))));
				
				retVal.AppendLine("</li>");
			}
			if(socialNetworks.Contains("in"))
			{
				retVal.AppendLine("<li class=\"list-item __in\">");

				retVal.AppendLine(
					string.Format(
					"<a target=\"_blank\" href=\"https://www.linkedin.com/shareArticle?mini=true&url={0}&title={1}&summary={2}&source={3}\" class=\"list-link\">Linkedin</a>",
					HttpUtility.UrlEncode(string.Format(SiteUrlHelper.ResolveServerUrl("~/modules/{0}"), module.Code)),
					module.Title,
					module.FullDescription,
					SiteUrlHelper.ResolveServerUrl("~")));
				
				retVal.AppendLine("</li>");
			}

			return MvcHtmlString.Create(retVal.ToString());
		}

		public static MvcHtmlString GetExtensionButton(this HtmlHelper htmlHelper, Module module)
		{
			var price = module.Price;
			if(price.IsFree)
			{
				return MvcHtmlString.Create(string.Format("Get Extension ( {0} )", price.FormatedPrice));
			}

			var format = "<span itemprop=\"offers\" itemscope itemtype=\"http://schema.org/Offer\">Get Extension ( {0} )</span>";
			var priceFormat = "<span itemprop=\"priceCurrency\" content=\"{0}\"></span>{1}<span itemprop=\"price\" content=\"{2}\"></span>";
			var priceHtml = string.Format(priceFormat, price.Currency, price.FormatedPrice, price.Price);
			return MvcHtmlString.Create(string.Format(format, priceHtml));
		}
	}
}