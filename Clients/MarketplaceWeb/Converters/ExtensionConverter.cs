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
        public const string PriceProperty = "price";
        public const string IsFreeProperty = "isFree";
        public const string CurrencyProperty = "currency";

        public static Extension ToWebModel(this CatalogItem item)
        {
            var retVal = new Extension {
                Code = item.Code, 
                Id = item.Id, 
                Name = item.Name,
                CatalogId = item.CatalogId,
                Images = item.Images
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


            if (item.Properties.ContainsKey(PriceProperty))
            {
                decimal price;
                decimal.TryParse(item[PriceProperty], out price);
                retVal.Price = price;
            }

            if (item.Properties.ContainsKey(IsFreeProperty))
            {
                bool isFree;
                bool.TryParse(item[IsFreeProperty], out isFree);
                retVal.IsFree = isFree;
            }

            if (item.Properties.ContainsKey(CurrencyProperty))
            {
                retVal.Currency = item[CurrencyProperty];
            }

            if (!retVal.IsFree && !string.IsNullOrWhiteSpace(retVal.Currency))
            {
                retVal.FormatedCurrency = CurrencyHelper.FormatCurrency(retVal.Price, retVal.Currency);
            }

            return retVal;
        }
    }
}