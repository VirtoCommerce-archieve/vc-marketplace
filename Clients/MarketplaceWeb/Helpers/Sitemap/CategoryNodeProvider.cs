using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcSiteMapProvider;
using VirtoCommerce.ApiClient.DataContracts;

namespace MarketplaceWeb.Helpers.Sitemap
{
    public class CategoryNodeProvider : DynamicNodeProviderBase
    {

        public static List<Category> FakeCategories = new List<Category>
        {
            new Category
            {
                Code = "CE",
                Id = Guid.NewGuid().ToString(),
                Name = "Customer Experience"
            },
            new Category
            {
                Code = "SM",
                Id = Guid.NewGuid().ToString(),
                Name = "Site Management"
            },
            new Category
            {
                Code = "INT",
                Id = Guid.NewGuid().ToString(),
                Name = "Integration"
            },
            new Category
            {
                Code = "MK",
                Id = Guid.NewGuid().ToString(),
                Name = "Marketing"
            },
            new Category
            {
                Code = "UTILS",
                Id = Guid.NewGuid().ToString(),
                Name = "Utilities"
            },
            new Category
            {
                Code = "THM",
                Id = Guid.NewGuid().ToString(),
                Name = "Themes"
            },
        };
        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            return FakeCategories.OrderByDescending(x => x.Name).Select(fakeCategory => new DynamicNode
            {
                Action = "Display",
                Title = fakeCategory.Name,
                Key = fakeCategory.Id,
                Order = FakeCategories.IndexOf(fakeCategory),
                ParentKey = fakeCategory.ParentId,
                RouteValues = new Dictionary<string, object> { { "categoryId", fakeCategory.Id } }
            });
        }
    }
}