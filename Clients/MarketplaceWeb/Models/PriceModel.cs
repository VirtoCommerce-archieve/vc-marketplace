using System;
using System.Collections.Generic;
using System.Linq;
using MarketplaceWeb.Helpers;

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

        public static PriceModel Parse(IDictionary<string, string[]> propertyDictionary)
        {
            var retVal = new PriceModel();

            var key = propertyDictionary.Keys.FirstOrDefault(x => x.Equals(PriceProperty, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(key))
            {
                decimal price;
                decimal.TryParse(propertyDictionary[PriceProperty].First(), out price);
                retVal.Price = price;
            }

            key = propertyDictionary.Keys.FirstOrDefault(x => x.Equals(IsFreeProperty, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(key))
            {
                bool isFree;
                bool.TryParse(propertyDictionary[IsFreeProperty].First(), out isFree);
                retVal.IsFree = isFree;
            }

            key = propertyDictionary.Keys.FirstOrDefault(x => x.Equals(CurrencyProperty, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(key))
            {
                retVal.Currency = propertyDictionary[CurrencyProperty].First();
            }

            if (!retVal.IsFree && !string.IsNullOrWhiteSpace(retVal.Currency))
            {
                retVal.FormatedPrice = CurrencyHelper.FormatCurrency(retVal.Price, retVal.Currency);
            }

            if (retVal.IsFree)
            {
                retVal.FormatedPrice = "Free";
            }

            return retVal;
        }
    }
}