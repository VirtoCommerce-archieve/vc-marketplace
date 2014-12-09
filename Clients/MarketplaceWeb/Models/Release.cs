using System;

namespace MarketplaceWeb.Models
{
    public class Release
    {
        public DateTime? ReleaseDate { get; set; }
        public Version Version { get; set; }
        public Version[] Compatibility { get; set; }
        public string Note { get; set; }
        public Uri DownloadLink { get; set; }
    }
}