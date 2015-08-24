using System;
using System.Collections.Generic;
using System.Linq;
using MarketplaceWeb.Helpers;
using MarketplaceWeb.Converters;

namespace MarketplaceWeb.Models
{
    public class PriceModel
    {
        public const string PriceProperty = "price";
        public const string IsFreeProperty = "isFree";
        public const string CurrencyProperty = "currency";

        public decimal Price { get; set; }

        public string Currency { get; set; }

        public bool IsFree { get; set; }

        public string FormatedPrice { get; set; }

        public static PriceModel Parse(IDictionary<string, object> propertyDictionary)
        {
            var retVal = new PriceModel();

            var key = propertyDictionary.Keys.FirstOrDefault(x => x.Equals(PriceProperty, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(key))
            {
                decimal price;
                decimal.TryParse(propertyDictionary.ParsePropertyToString(PriceProperty), out price);
                retVal.Price = price;
            }

            key = propertyDictionary.Keys.FirstOrDefault(x => x.Equals(IsFreeProperty, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(key))
            {
                bool isFree;
                bool.TryParse((string)propertyDictionary[IsFreeProperty], out isFree);
                retVal.IsFree = isFree;
            }

            key = propertyDictionary.Keys.FirstOrDefault(x => x.Equals(CurrencyProperty, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(key))
            {
                retVal.Currency = (string)propertyDictionary[CurrencyProperty];
            }

            if (!retVal.IsFree && !string.IsNullOrWhiteSpace(retVal.Currency))
            {
                retVal.FormatedPrice = CurrencyHelper.FormatCurrency(retVal.Price, retVal.Currency);
            }

			if (retVal.IsFree || retVal.Price == 0)
            {
                retVal.FormatedPrice = "Free";
            }

            return retVal;
        }
    }
}