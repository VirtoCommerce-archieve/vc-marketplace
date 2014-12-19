namespace MarketplaceWeb.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string FullDescription { get; set; }

        public UserType UserType { get; set; }

        /// <summary>
        /// Icon is byte[] encoded as base64
        /// </summary>
        public string Icon { get; set; }
    }

    public enum UserType
    {
        Customer = 0, //Registered used that has 0 accepted extensions
        Developer, //Registered user that has accepted extensions
        Admin
    }
}