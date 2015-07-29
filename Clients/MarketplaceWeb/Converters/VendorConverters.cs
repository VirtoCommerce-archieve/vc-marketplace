using MarketplaceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.ApiClient.DataContracts.CustomerService;
using VirtoCommerce.ApiClient.Extensions;
using MarketplaceWeb.Helpers;

namespace MarketplaceWeb.Converters
{
	public static class VendorConverters
	{
		public static Vendor ToWebModel(this Contact contact)
		{
			var vendor = new Vendor();

			if (contact != null)
			{
				vendor.Id = contact.Id;
				vendor.Name = contact.FullName;

				if (contact.DynamicProperties != null && contact.DynamicProperties.Any())
				{
					vendor.Icon = contact.GetPropertyValue("Icon");
					vendor.Description = contact.GetPropertyValue("Description");
					vendor.FullDescription = contact.GetPropertyValue("FullDescription");
					vendor.UserEmail = contact.GetPropertyValue("Email");
				}
			}

			return vendor;
		}
	}
}