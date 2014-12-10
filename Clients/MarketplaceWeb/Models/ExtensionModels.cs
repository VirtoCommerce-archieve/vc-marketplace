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
        public string Id { get; set; }

        public string CatalogId { get; set; }

        public string Code { get; set; }

        public string Title { get; set; }

        public PriceModel Price { get; set; }

        public string Description { get; set; }

        public string FullDescription { get; set; }

        public string License { get; set; }

        public ItemImage[] Images { get; set; }

        public int ReviewsTotal { get; set; }

        public double Rating { get; set; }

        public string Locale { get; set; }

        public IEnumerable<string> Compatibility
        {
            get
            {
                if (Releases != null)
                {
                    return Releases.SelectMany(x => x.Compatibility).Distinct();
                }

                return null;
            }
        }

        public Release[] Releases { get; set; }
    }

    public class CustomPropertyCollection : Collection<CustomProperty>
    {
        public static CustomPropertyCollection Parse(IDictionary<string, string[]> propertyDictionary, IDictionary<string, string> keyDictionary)
        {
            var retVal = new CustomPropertyCollection();
            foreach (var keyValue in propertyDictionary)
            {
                var displayKey = keyDictionary.FirstOrDefault(k => k.Key.Equals(keyValue.Key, StringComparison.OrdinalIgnoreCase));
                if (!displayKey.Equals(default(KeyValuePair<string,string>)))
                {
                    retVal.Add(CustomProperty.Parse(new KeyValuePair<string, string[]>(displayKey.Value, keyValue.Value)));
                }
            }

            return retVal;
        }
    }

    public class CustomProperty
    {

        public static CustomProperty Parse(KeyValuePair<string, string[]> valuePair)
        {
            return new CustomProperty
            {
                DisplayName = valuePair.Key,
                DisplayValue = string.Join(", ", valuePair.Value)
            };
        }

        public string DisplayName { get; set; }

        public string DisplayValue { get; set; }
    }
}