using System;
using System.Collections.Generic;
using System.Linq;
using MarketplaceWeb.Helpers;
using MarketplaceWeb.Converters;
using VirtoCommerce.Client.Model;

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

        public static PriceModel Parse(List<VirtoCommerceCatalogModuleWebModelProperty> properties)
        {
            var retVal = new PriceModel();

            var key = properties.Select(p => p.Name).FirstOrDefault(x => x.Equals(PriceProperty, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(key))
            {
                decimal price;
                decimal.TryParse(properties.ParsePropertyToString(PriceProperty), out price);
                retVal.Price = price;
            }

            key = properties.Select(p => p.Name).FirstOrDefault(x => x.Equals(IsFreeProperty, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(key))
            {
                bool isFree;
                var property = properties.FirstOrDefault(p => p.Name == IsFreeProperty);
                if (property != null && property.Values.Count > 0)
                {
                    if(bool.TryParse((string)property.Values.FirstOrDefault().Value, out isFree))
                        retVal.IsFree = isFree;
                }
            }

            key = properties.Select(p => p.Name).FirstOrDefault(x => x.Equals(CurrencyProperty, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(key))
            {
                var property = properties.FirstOrDefault(p => p.Name == CurrencyProperty);
                if (property != null && property.Values.Count > 0)
                {
                    retVal.Currency = (string)property.Values.FirstOrDefault().Value;
                }
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