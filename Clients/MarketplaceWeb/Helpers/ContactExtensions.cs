using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.ApiClient.DataContracts.CustomerService;

namespace MarketplaceWeb.Helpers
{
	public static class ContactExtensions
	{
		public static string GetPropertyValue(this Contact contact, string propertyName)
		{
			var property = contact.DynamicProperties.FirstOrDefault(p => p.Name == propertyName);

			if (property == null && !property.Values.Any())
				return string.Empty;

			return property.Values.First().Value;
		}
	}
}