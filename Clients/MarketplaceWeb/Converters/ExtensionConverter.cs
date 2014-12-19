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
                Name = "Test developer",
                Description = "I'm a deployment & integrations provider, I use Enterprise",
                Icon =
                    @"/9j/4AAQSkZJRgABAQAAAQABAAD/2wCEAAkGBwgHBgkIBwgKCgkLDRYPDQwMDRsUFRAWIB0iIiAdHx8kKDQsJCYxJx8fLT0tMTU3Ojo6Iys/RD84QzQ5OjcBCgoKDQwNGg8PGjclHyU3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3N//AABEIAHcAeAMBIgACEQEDEQH/xAAcAAEBAQADAQEBAAAAAAAAAAAABwYBBAUIAwL/xAA9EAABAwICBgcEBgsAAAAAAAAAAQIDBBEFBgcSITFBgRdRVWGRlNITQnGhFCIjMmKxFSQzUlRyc6LB0fD/xAAUAQEAAAAAAAAAAAAAAAAAAAAA/8QAFBEBAAAAAAAAAAAAAAAAAAAAAP/aAAwDAQACEQMRAD8A7Gd84V2J4lPSUVRJBQQvWNrY3K1ZbbFc5U3ovBN1jHgAAAAAAAAAAAAAAAAAbDI+cK3DMSgpK2oknoJnoxzZXK5Yrrsc1V3J1puBj924AAAAAAAA7WF4dVYrXRUVBEsk8i7E4InFVXgneB1QWHAtGuFUcbH4prV9RvVFVWxovc1N/PwPfXKeXlZqrgtBb+g2/iB8/grGYdGdDPE+XA3LSzptSF7ldG7uuu1v5dxLKumno6mSmqonRTxO1Xscm1FA/IAAAAAAAAAAAAAK9omwiOmwR2Juaiz1j1RHKm1sbVsic1RV8OokJe8iWTKGFW/h0A94AACcaXsGjdSQYzE1EljckMy/vNX7q8l2cyjmV0nOamS67WS93woid/tGgRAAAAAAAAAAAAAALVotrkq8pww615KWR8TvhfWT5OTwIqbHRjjrcKxtaSodamrrMuu5sifdXne3NALQDhDkAYDTBXJFgtJRIqa9RPrqn4WJ/tUN8q2ITn3G245mGWWF2tSwJ7GBU3ORF2u5rflYDOgAAAAAAAAAAAERVVERFVVWyInEAdmjw+tr7pRUdRUW3+xic63NCiZO0dNWNlbmJiq5yXZR3sifz9a/h8SkQQx08TYoI2RxtSzWMajURO5EAyGQs1/pKH9F4q5Y8Vp/qqkv1XTInGy+91pz+GzPAzLlOgx9qSS61PWs/Z1UWx7bbr9af8ioZ5uUc1yfqtRmuT6HuVzdb2ip8l/uUD+tIOaXIx+A4Jrz186K2ZYEVzo28Wpb3l+SEoqKeekl9jVQSQSJ7kjFavgpfcvZbw3L8GpQRfaOT7Sd+17/AIr/AITYd3EMOo8Tp1p6+miniX3ZG3t8Ope9APnEG/zho8loGSVuB689O3a+mct3sTravvJ3b/iYAAAAAAAAAAUnRZlhstsdrmXRHKlIxycU3v8AHYnPuJta+xF1b7LrwLRQ55ylQUUFJT1z2xQRtjYn0aXciWT3QNgcmV6RMr9oP8tL6R0iZX7Qf5WX0gaoGV6RMr9oP8rL6R0iZX7Qf5aX0gaoGV6RMr9oP8tL6R0iZX7Qf5aX0galSR6Tsrsw2oTFqCNG0079WdjU2RyLxTuX8/ibPpEyv2g/ysvpOljGc8pYthlTQT4g/UnjVmt9Fl+qvBfu8FsvICOgJy5AAAAAOXNVrla5FRUWyovA4AAAAAAAAAAAAAAAAAA5a1XORrUuqrZE6wBv89ZHqoa2oxPCmNkpZXLJLHrI10Tl32vvS+3rJ+uxbLvAAAAAAAAAAAAAAAATatk3gAUDIeR6qatp8UxVjY6WJySRR6yOWVyblW25L81AAH//2Q=="
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