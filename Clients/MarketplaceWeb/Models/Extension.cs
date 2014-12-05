using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.ApiClient.DataContracts;

namespace MarketplaceWeb.Models
{
    public class Extension
    {

        public string Id { get; set; }

        public string CatalogId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public bool IsFree { get; set; }

        public decimal Price { get; set; }

        public string Currency { get; set; }

        public string FormatedCurrency { get; set; }

        public string Description { get; set; }

        public string FullDescription { get; set; }

        public ItemImage[] Images { get; set; }

        public ItemImage PrimaryImage
        {
            get { return Images != null ? Images.FirstOrDefault() : null; }
        }
    }
}