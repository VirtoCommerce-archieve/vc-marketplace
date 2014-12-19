using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarketplaceWeb.Models
{
    public class DeveloperProfile
    {
        public User User { get; set; }

        public IEnumerable<Extension> Extensions { get; set; }
    }
}