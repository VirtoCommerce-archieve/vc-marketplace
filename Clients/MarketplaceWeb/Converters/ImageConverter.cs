using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using clientModels = VirtoCommerce.Client.Model;
using webModels = MarketplaceWeb.Models;

namespace MarketplaceWeb.Converters
{
    public static class ImageConverter
    {
        public static webModels.ItemImage ToWebModel(this clientModels.VirtoCommerceMerchandisingModuleWebModelImage image)
        {
            var retVal = new webModels.ItemImage
            {
                Attachement = image.Attachement,
                Group = image.Group,
                Name = image.Name,
                Src = image.Src,
                ThumbSrc = image.ThumbSrc
            };

            return retVal;
        }

        public static webModels.ItemImage ToWebModel(this clientModels.VirtoCommerceCatalogModuleWebModelImage image)
        {
            var retVal = new webModels.ItemImage
            {
                Group = image.Group,
                Name = image.Name,
                Src = image.Url,
                ThumbSrc = image.Url
            };

            return retVal;
        }
    }
}