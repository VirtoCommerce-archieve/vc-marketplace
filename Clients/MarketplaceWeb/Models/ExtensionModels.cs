using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using VirtoCommerce.ApiClient.DataContracts;

namespace MarketplaceWeb.Models
{
    public class Extension
    {

        /// <summary>
        /// Product id
        /// </summary>
        public string Id { get; set; }

        public string CatalogId { get; set; }

        /// <summary>
        /// Category ids where extension is placed
        /// </summary>
        public List<string> CategoryList { get; set; }

        public string Code { get; set; }

        public string Title { get; set; }

        public PriceModel Price { get; set; }

        public string Description { get; set; }

        public string FullDescription { get; set; }

        public string License { get; set; }

        public List<ItemImage> Images { get; set; }

        public int ReviewsTotal { get; set; }

        public double Rating { get; set; }

        public List<string> Locale { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

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
    }
}