namespace VirtoCommerce.ApiClient.DataContracts.Security
{
	public class AuthInfo
	{
		public string Login { get; set; }
		public string FullName { get; set; }
		public string[] Permissions { get; set; }

        public RegisterType UserType { get; set; }
	}
}