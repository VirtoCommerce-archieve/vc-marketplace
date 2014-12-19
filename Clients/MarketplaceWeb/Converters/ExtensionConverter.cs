using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web;
using MarketplaceWeb.Helpers;
using MarketplaceWeb.Models;
using VirtoCommerce.ApiClient.DataContracts;

namespace MarketplaceWeb.Converters
{
    public static class ExtensionConverter
    {
        public const string LicenseProperty = "License";
        public const string LocaleProperty = "Locale";
        public const string UserIdProperty = "UserId";

        //Variation properties
        public const string CompatibilityProperty = "Compatibility";
        public const string ReleaseDateProperty = "ReleaseDate";
        public const string ReleaseStatusProperty = "ReleaseStatus";
        public const string LinkProperty = "DowloadLink";
        public const string NoteProperty = "ReleaseNote";
        public const string VersionProperty = "ReleaseVersion";


        public static Extension ToWebModel(this Product item)
        {
            var retVal = new Extension
            {
                Code = item.Code,
                Id = item.Id,
                Title = item.Name,
                CatalogId = item.CatalogId,
                Images = item.Images.ToList(),
                Rating = item.Rating,
                ReviewsTotal = item.ReviewsTotal,
                Price = PriceModel.Parse(item.Properties),
                CategoryList = item.Categories != null ? item.Categories.ToList() : null
            };

            if (item.EditorialReviews != null)
            {
                var reviews = item.EditorialReviews.Where(x => !string.IsNullOrWhiteSpace(x.ReviewType)).ToArray();
                var shortReview = reviews.FirstOrDefault(
                    x => x.ReviewType.Equals("QuickReview", StringComparison.OrdinalIgnoreCase));

                var fullReview = reviews.FirstOrDefault(
                    x => x.ReviewType.Equals("FullReview", StringComparison.OrdinalIgnoreCase));

                if (shortReview != null)
                {
                    retVal.Description = shortReview.Content;
                }

                if (fullReview != null)
                {
                    retVal.FullDescription = fullReview.Content;
                }
            }

            if (item.Properties != null)
            {
                retVal.License = item.Properties.ParsePropertyToString(LicenseProperty);
                retVal.Locale = item.Properties.ParseProperty(LocaleProperty).ToList();
                retVal.UserId = item.Properties.ParsePropertyToString(UserIdProperty);
            }

            if (item.Variations != null)
            {
                retVal.Releases = item.Variations.Select(x => x.ToWebModel(retVal)).ToList();
            }

            //TODO replace hardcoded developer when api service available
            retVal.User = new User
            {
                Id = "1",
                Name = "Virto Commerce",
                Description = "Virto commerce is a solution provider",
                Icon =
                    @"iVBORw0KGgoAAAANSUhEUgAAAJYAAACWCAYAAAA8AXHiAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyJpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMy1jMDExIDY2LjE0NTY2MSwgMjAxMi8wMi8wNi0xNDo1NjoyNyAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoV2luZG93cykiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6MjExRkU2QzM4NzZEMTFFNDlFMUFEM0Y1QzY5MDQ3MTMiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6MjExRkU2QzQ4NzZEMTFFNDlFMUFEM0Y1QzY5MDQ3MTMiPiA8eG1wTU06RGVyaXZlZEZyb20gc3RSZWY6aW5zdGFuY2VJRD0ieG1wLmlpZDoyMTFGRTZDMTg3NkQxMUU0OUUxQUQzRjVDNjkwNDcxMyIgc3RSZWY6ZG9jdW1lbnRJRD0ieG1wLmRpZDoyMTFGRTZDMjg3NkQxMUU0OUUxQUQzRjVDNjkwNDcxMyIvPiA8L3JkZjpEZXNjcmlwdGlvbj4gPC9yZGY6UkRGPiA8L3g6eG1wbWV0YT4gPD94cGFja2V0IGVuZD0iciI/PqOJYJ0AAA2wSURBVHja7J19cBT1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"
            };

            return retVal;
        }

        public static Release ToWebModel(this CatalogItem variation, Extension parent)
        {
            var retVal = new Release
            {
                Id = variation.Id,
                Compatibility = variation.Properties.ParseProperty(CompatibilityProperty).ToList(),
                DownloadLink = variation.Properties.ParsePropertyToString(LinkProperty),
                ReleaseDate = variation.Properties.ParseProperty<DateTime>(ReleaseDateProperty).FirstOrDefault(),
                Note = variation.Properties.ParsePropertyToString(NoteProperty),
                Version = variation.Properties.ParsePropertyToString(VersionProperty),
                ReleaseStatus = variation.Properties.ParseProperty<ReleaseStatus>(ReleaseStatusProperty).FirstOrDefault(),
                ParentExtension = parent
            };

            return retVal;
        }

        public static string[] ParseProperty(this IDictionary<string, string[]> properties, string name)
        {
            var key = properties.Keys.FirstOrDefault(x => x.Equals(name, StringComparison.OrdinalIgnoreCase));
            return !string.IsNullOrEmpty(key) ? properties[key] : new string[0];
        }

        public static T[] ParseProperty<T>(this IDictionary<string, string[]> properties, string name)
        {
            var retVal = new List<T>();
            var values = ParseProperty(properties, name);
            if (values != null && values.Any())
            {
                var type = typeof(T);


                var tc = TypeDescriptor.GetConverter(type);

                foreach (var value in values)
                {
                    try
                    {
                        var result = (T)tc.ConvertFromString(null, CultureInfo.InvariantCulture, value);
                        retVal.Add(result);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            return retVal.ToArray();
        }

        public static string ParsePropertyToString(this IDictionary<string, string[]> properties, string name,
            string separator = ", ")
        {
            var values = ParseProperty(properties, name);

            if (values != null)
            {
                return values.Length > 1 ? string.Join(separator, values) : values.FirstOrDefault();
            }

            return null;
        }



    }
}