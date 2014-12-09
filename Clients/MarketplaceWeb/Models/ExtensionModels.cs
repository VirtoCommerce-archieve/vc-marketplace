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

        public string Title { get; set; }

        public PriceModel Price { get; set; }

        public string Description { get; set; }

        public string FullDescription { get; set; }

        public string License { get; set; }

        public ItemImage[] Images { get; set; }

        public int ReviewsTotal { get; set; }

        public decimal Rating { get; set; }

        public string[] Locale { get; set; }

        public Release[] Releases { get; set; }
    }
}