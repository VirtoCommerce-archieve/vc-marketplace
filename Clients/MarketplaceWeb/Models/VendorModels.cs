using System.Collections.ObjectModel;
namespace MarketplaceWeb.Models
{
    public class Vendor
    {
		public Vendor()
		{
			this.Seo = new Seo();
		}

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string FullDescription { get; set; }

        public UserType UserType { get; set; }

		public string Site { get; set; }

        /// <summary>
        /// Icon is byte[] encoded as base64
        /// </summary>
        public string Icon { get; set; }

		public string UserEmail { get; set; }

		private Collection<Module> _modules;
		public Collection<Module> Modules { get { return _modules ?? (_modules = new Collection<Module>()); } }

		public Seo Seo { get; set; }
	}

    public enum UserType
    {
        Customer = 0, //Registered used that has 0 accepted extensions
        Developer, //Registered user that has accepted extensions
        Admin
    }

	public class Seo
	{
		public string Title { get; set; }
		public string MetaDescription { get; set; }
	}
}