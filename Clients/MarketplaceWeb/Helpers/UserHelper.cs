using MarketplaceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.DataContracts.CustomerService;
using VirtoCommerce.ApiClient.Extensions;
using MarketplaceWeb.Converters;
using VirtoCommerce.ApiClient.DataContracts;

namespace MarketplaceWeb.Helpers
{
	public class UserHelper
	{
		public CustomerServiceClient CustomerServiceClient
		{
			get
			{
				return ClientContext.Clients.CreateCustomerServiceClient();
			}
		}

		public async Task<Vendor> GetUser(Product item)
		{
			var user = new Vendor();

			var vendorId = item.Properties.ParsePropertyToString("VendorId");

			if(!string.IsNullOrEmpty(item.Properties.ParsePropertyToString("VendorId")))
			{
				user = await GetUser(vendorId);
			}

			return user;
		}

		public async Task<Vendor> GetUser(string vendorId)
		{
			var user = new Vendor();

			var contact = await CustomerServiceClient.GetContactByIdAsync(vendorId);

			if(contact != null)
			{
				user.Id = contact.Id;
				user.Name = contact.FullName;

				user.Icon = contact.GetPropertyValue("Icon");
				user.Description = contact.GetPropertyValue("Description");
				user.FullDescription = contact.GetPropertyValue("FullDescription");
				user.UserEmail = contact.GetPropertyValue("Email");

				user.Seo.Title = contact.GetPropertyValue("Title");
				user.Seo.MetaDescription = contact.GetPropertyValue("MetaDescription");
			}

			return user;
		}
	}
}