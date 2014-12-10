using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MarketplaceWeb.Helpers;
using MarketplaceWeb.Models;
using VirtoCommerce.ApiClient.DataContracts;

namespace MarketplaceWeb.Converters
{
    public static class ExtensionConverter
    {
        public const string LicenseProperty = "license";
        public const string LocaleProperty = "locale";


        public static Extension ToWebModel(this CatalogItem item)
        {
            var retVal = new Extension {
                Code = item.Code, 
                Id = item.Id, 
                Title = item.Name,
                CatalogId = item.CatalogId,
                Images = item.Images,
                Rating = item.Rating,
                ReviewsTotal = item.ReviewsTotal,
                Price = PriceModel.Parse(item.Properties)
            };

            if (item.EditorialReviews != null)
            {
                var shortReview = item.EditorialReviews.FirstOrDefault(
                    x => x.ReviewType.Equals("QuickReview", StringComparison.OrdinalIgnoreCase));

                var fullReview = item.EditorialReviews.FirstOrDefault(
                    x => x.ReviewType.Equals("FullReview", StringComparison.OrdinalIgnoreCase));

                if (shortReview !=null)
                {
                    retVal.Description = shortReview.Content;
                }

                if (fullReview != null)
                {
                    retVal.FullDescription = fullReview.Content;
                }               
            }

            if (item.Properties != null)
            {
                var key = item.Properties.Keys.FirstOrDefault(x => x.Equals(LicenseProperty, StringComparison.OrdinalIgnoreCase));
                if (!string.IsNullOrEmpty(key))
                {
                    retVal.License = item.Properties[key].First();
                }

                key = item.Properties.Keys.FirstOrDefault(x => x.Equals(LocaleProperty, StringComparison.OrdinalIgnoreCase));
                if (!string.IsNullOrEmpty(key))
                {
                    retVal.Locale = string.Join(", ", item.Properties[key]);
                }
            }

            return retVal;
        }
    }
}