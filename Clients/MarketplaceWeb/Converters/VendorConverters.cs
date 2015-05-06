using MarketplaceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.ApiClient.DataContracts.CustomerService;
using VirtoCommerce.ApiClient.Extensions;

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

				vendor.Icon = contact.Properties.TryGetValue("Icon");
				vendor.Description = contact.Properties.TryGetValue("Description");
				vendor.FullDescription = contact.Properties.TryGetValue("FullDescription");
				vendor.UserEmail = contact.Properties.TryGetValue("Email");
			}

			return vendor;
		}
	}
}