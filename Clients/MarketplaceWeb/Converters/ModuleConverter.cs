using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web;
using MarketplaceWeb.Helpers;
using MarketplaceWeb.Models;
using VirtoCommerce.ApiClient.DataContracts;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace MarketplaceWeb.Converters
{
	public static class ModuleConverter
	{
		public const string LicenseProperty = "License";
		public const string LocaleProperty = "Locale";
		public const string UserIdProperty = "VendorId";
		public const string DescriptionProperty = "Description";

		//Variation properties
		public const string CompatibilityProperty = "Compatibility";
		public const string ReleaseDateProperty = "ReleaseDate";
		public const string ReleaseStatusProperty = "ReleaseStatus";
		public const string LinkProperty = "DowloadLink";
		public const string NoteProperty = "ReleaseNote";
		public const string VersionProperty = "ReleaseVersion";


		public static Module ToWebModel(this Product item)
		{
			var retVal = new Module
			{
				Code = item.Code,
				Id = item.Id,
				Title = item.Name,
				CatalogId = item.CatalogId,
				Images = item.Images.ToList(),
				ReviewsTotal = item.ReviewsTotal,
				Price = PriceModel.Parse(item.Properties),
				//CategoryList = item.Categories != null ? item.Categories.ToList() : null
			};

			if (item.EditorialReviews != null)
			{
				var reviews = item.EditorialReviews.Where(x => !string.IsNullOrWhiteSpace(x.ReviewType)).ToArray();
				var shortReview = reviews.FirstOrDefault(
					x => x.ReviewType.Equals("QuickReview", StringComparison.OrdinalIgnoreCase));

				var fullReview = reviews.FirstOrDefault(
					x => x.ReviewType.Equals("FullReview", StringComparison.OrdinalIgnoreCase));

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
				retVal.Description = item.Properties.ParsePropertyToString(DescriptionProperty);
			}

			if (item.Variations != null)
			{
				retVal.Releases = item.Variations.Select(x => x.ToWebModel(retVal)).ToList();
			}

			return retVal;
		}

		public static Release ToWebModel(this CatalogItem variation, Module parent)
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

		public static string[] ParseProperty(this IDictionary<string, object> properties, string name)
		{
			var key = properties.Keys.FirstOrDefault(x => x.Equals(name, StringComparison.OrdinalIgnoreCase));
			if (!string.IsNullOrEmpty(key))
			{
				if (properties[key] is string[])
				{
					return (string[])properties[key];
				}
				else if (properties[key] is JArray)
				{
					return JsonConvert.DeserializeObject<string[]>(properties[key].ToString());
				}
				else
				{
					return new string[] { properties[key].ToString() };
				}
			}
			else
			{
				return new string[0];
			}
		}

		public static T[] ParseProperty<T>(this IDictionary<string, object> properties, string name)
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

		public static string ParsePropertyToString(this IDictionary<string, object> properties, string name,
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