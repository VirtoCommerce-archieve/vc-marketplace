using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace MarketplaceWeb.Models
{
	public class CategoryMenu
	{
		private Collection<ShortCategoryInfo> _categories;
		public Collection<ShortCategoryInfo> Categories { get { return _categories ?? (_categories = new Collection<ShortCategoryInfo>()); } }
	}

	public class ShortCategoryInfo
	{
		public string CategoryName { get; set; }
		public string CategoryCode { get; set; }
	}

	public class CategoryResults
	{
		public string CategoryName { get; set; }
		public string CategoryCode { get; set; }

		private Collection<Module> _modules;
		public Collection<Module> Modules { get { return _modules ?? (_modules = new Collection<Module>()); } }
	}
}