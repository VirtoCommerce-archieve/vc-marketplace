namespace VirtoCommerce.ApiClient.DataContracts.Security
{
	public class UserLogin
	{
		public string UserName { get; set; }
		public string Password { get; set; }
		public bool RememberMe { get; set; }
	}
}
