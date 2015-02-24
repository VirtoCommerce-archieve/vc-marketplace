using MarketplaceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.DataContracts.CustomerService;
using VirtoCommerce.Web.Core.DataContracts;
using VirtoCommerce.ApiClient.Extensions;
using MarketplaceWeb.Converters;

namespace MarketplaceWeb.Helpers
{
	public class UserHelper
	{
		public CustomerServiceClient CustomerServiceClient
		{
			get
			{
				return ClientContext.Clients.CreateDefaultCustomerServiceClient();
			}
		}

		public async Task<User> GetUser(Product item)
		{
			var user = new User();

			var vendorId = item.Properties.ParsePropertyToString("VendorId");

			if(!string.IsNullOrEmpty(item.Properties.ParsePropertyToString("VendorId")))
			{
				user = await GetUser(vendorId);
			}

			return user;
		}

		public async Task<User> GetUser(string vendorId)
		{
			var user = new User();

			var contact = await CustomerServiceClient.GetContactById(vendorId);

			if(contact != null)
			{
				user.Id = contact.Id;
				user.Name = contact.FullName;

				user.Icon = contact.Properties.TryGetValue("Icon");
				user.Description = contact.Properties.TryGetValue("Description");
				user.FullDescription = contact.Properties.TryGetValue("FullDescription");
				user.UserEmail = contact.Properties.TryGetValue("Email");
			}

			return user;
		}
	}
}