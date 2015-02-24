using MarketplaceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MarketplaceWeb.Converters;
using VirtoCommerce.Web.Core.DataContracts;
using MarketplaceWeb.Helpers;

namespace MarketplaceWeb.Controllers
{
	[RoutePrefix("vendor")]
	public class VendorController : ControllerBase
	{
		// GET: Vendor
		public ActionResult Index()
		{
			return View();
		}

		[Route("{vendorId}")]
		public async Task<ActionResult> Information(string vendorId, BrowseQuery query)
		{
			var model = new VendorInformationModel();

			var userHelper = new UserHelper();

			model.User = await userHelper.GetUser(vendorId);

			query.Filters.Add("vendorId", new[] { vendorId });
			var results = await SearchClient.GetProductsAsync(query);

			model.Extensions = results.Items.Select(x => x.ToWebModel());

			return View(model);
		}
	}
}