using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.ApiClient.DataContracts.Contents
{
    public class DynamicContentItemGroup
    {
        public DynamicContentItemGroup(string groupName)
        {
            GroupName = groupName;
            Items = new List<DynamicContentItem>();
        }

        public List<DynamicContentItem> Items { get; private set; }

        public string GroupName { get; set; }
    }
}
