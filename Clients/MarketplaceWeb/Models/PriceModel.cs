using System.Collections.Generic;
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

        public static PriceModel Parse(IDictionary<string, string> propertyDictionary)
        {
            var retVal = new PriceModel();

            if (propertyDictionary.ContainsKey(PriceProperty))
            {
                decimal price;
                decimal.TryParse(propertyDictionary[PriceProperty], out price);
                retVal.Price = price;
            }

            if (propertyDictionary.ContainsKey(IsFreeProperty))
            {
                bool isFree;
                bool.TryParse(propertyDictionary[IsFreeProperty], out isFree);
                retVal.IsFree = isFree;
            }

            if (propertyDictionary.ContainsKey(CurrencyProperty))
            {
                retVal.Currency = propertyDictionary[CurrencyProperty];
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