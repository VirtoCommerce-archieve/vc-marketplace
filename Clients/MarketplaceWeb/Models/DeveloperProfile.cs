using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarketplaceWeb.Models
{
	public class VendorInformationModel
	{
		public VendorInformationModel()
		{
			User = new User();
		}

		public User User { get; set; }

		public IEnumerable<Extension> Extensions { get; set; }
	}
}