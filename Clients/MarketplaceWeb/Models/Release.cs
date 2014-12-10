using System;
using System.Collections.Generic;

namespace MarketplaceWeb.Models
{
    public class Release
    {
        public DateTime? ReleaseDate { get; set; }
        public string Version { get; set; }
        public List<string> Compatibility { get; set; }
        public string Note { get; set; }
        public string DownloadLink { get; set; }
    }
}