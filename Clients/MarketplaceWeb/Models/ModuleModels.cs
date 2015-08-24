using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using VirtoCommerce.ApiClient.DataContracts;

namespace MarketplaceWeb.Models
{
	public class ModulesModel
	{
		private Collection<Module> _items;
		public Collection<Module> Items { get { return _items ?? (_items = new Collection<Module>()); } }
	}

	public class Module
	{
		public Module()
		{
			CategoryList = new Dictionary<string, string>();
			Releases = new List<Release>();
            Images = new List<ItemImage>();
		}

		/// <summary>
		/// Product id
		/// </summary>
		public string Id { get; set; }

		public string CatalogId { get; set; }

		private Collection<ReviewModel> _reviews;
		public Collection<ReviewModel> Reviews { get { return _reviews ?? (_reviews = new Collection<ReviewModel>()); } }

		/// <summary>
		/// Category ids where extension is placed
		/// </summary>
		public Dictionary<string, string> CategoryList { get; set; }

		public string Code { get; set; }

		public string Keyword { get; set; }

		public string Title { get; set; }

		public PriceModel Price { get; set; }

		public string Description { get; set; }

		public string FullDescription { get; set; }

		public string Overview { get; set; }

		public string DownloadLink { get; set; }

		public bool IsDownloadable { get { return !string.IsNullOrEmpty(this.DownloadLink); } }

		public List<ItemImage> Images { get; set; }

		public int ReviewsTotal { get; set; }

		public string License { get; set; }

		public bool IsLicenseAvailable { get { return !string.IsNullOrEmpty(this.License); } }

		public double Rating
		{
			get
			{
				if (this.Reviews.Count > 0)
				{
					return this.Reviews.Sum(r => r.Rating) / this.Reviews.Count;
				}
				return 0;
			}
		}

		public List<string> Locale { get; set; }

		public string UserId { get; set; }

		public Vendor Vendor { get; set; }

		public List<Release> Releases { get; set; }

		public ReleaseStatus ReleaseStatus
		{
			get
			{
				return HasRelease ? LatestRelease.ReleaseStatus : ReleaseStatus.Draft;
			}
		}

		public bool HasRelease
		{
			get { return Releases != null && Releases.Any(); }
		}

		public IEnumerable<string> Compatibility
		{
			get
			{
				if (HasRelease)
				{
					return Releases.SelectMany(x => x.Compatibility).Distinct();
				}

				return new string[0];
			}
		}

		public Release LatestRelease
		{
			get
			{
				if (HasRelease)
				{
					return Releases.OrderByDescending(x => x.ReleaseDate).First();
				}

				return null;
			}
		}

		public List<long> Time { get; set; }
	}
}