using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.ApiClient.DataContracts;

namespace MarketplaceWeb.Models
{
    public class FullExtension
    {
        public Extension Extension { get; set; }

        public ICollection<ReviewModel> Reviews { get; set; }

        public int ReviewTotal
        {
            get
            {
                return Reviews != null ? Reviews.Count : 0;
            }
        }

        public double ReviewsAverage
        {
            get
            {
                return Reviews != null ? Reviews.Select(x => x.Rating).Average() : 0;
            }
        }
    }
}