using MarketplaceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Client.Model;
using MarketplaceWeb.Helpers;

namespace MarketplaceWeb.Converters
{
	public static class VendorConverter
	{
		public static Vendor ToWebModel(this VirtoCommerceCustomerModuleWebModelContact contact)
		{
            var vendor = new Vendor()
            {
                Id = contact.Id,
                Name = contact.FullName
            };

			if (contact.DynamicProperties != null && contact.DynamicProperties.Any())
			{
                vendor.Icon = contact.GetPropertyValue("Icon");
                vendor.Description = contact.GetPropertyValue("Description");
                vendor.FullDescription = contact.GetPropertyValue("FullDescription");
                vendor.UserEmail = contact.GetPropertyValue("Email");
                vendor.Site = contact.GetPropertyValue("Site");

                vendor.Seo.Title = contact.GetPropertyValue("Title");
                vendor.Seo.MetaDescription = contact.GetPropertyValue("MetaDescription");
			}

			return vendor;
		}
	}
}