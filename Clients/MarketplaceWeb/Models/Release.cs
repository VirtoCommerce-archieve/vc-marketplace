using System;
using System.Collections.Generic;

namespace MarketplaceWeb.Models
{
    public class Release
    {
        public string Id { get; set; }
        /// <summary>
        /// Should this field come from LastModified?
        /// </summary>
        public DateTime? ReleaseDate { get; set; }

        /// <summary>
        /// From property
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// From multi value property
        /// </summary>
        public List<string> Compatibility { get; set; }

        /// <summary>
        /// From editorial review
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// From property or asset?
        /// </summary>
        public string DownloadLink { get; set; }

        /// <summary>
        /// Parsed from property Status
        /// </summary>
        public ReleaseStatus ReleaseStatus { get; set; }

        public Extension ParentExtension { get; set; }
    }
}