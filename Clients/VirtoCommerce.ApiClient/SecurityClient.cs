using System;
using System.Net.Http;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient.DataContracts;
using VirtoCommerce.ApiClient.DataContracts.Security;
using VirtoCommerce.ApiClient.Utilities;

namespace VirtoCommerce.ApiClient
{
    using VirtoCommerce.ApiClient.DataContracts.Contents;

    public class SecurityClient : BaseClient
    {
        protected class RelativePaths
        {
            public const string Login = "login";
        }

        /// <summary>
        /// Initializes a new instance of the AdminManagementClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
        /// <param name="token">Access token</param>
        public SecurityClient(Uri adminBaseEndpoint, string token)
            : base(adminBaseEndpoint, new TokenMessageProcessingHandler(token))
        {
        }

        /// <summary>
        /// Initializes a new instance of the AdminManagementClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
        /// <param name="handler"></param>
        public SecurityClient(Uri adminBaseEndpoint, MessageProcessingHandler handler)
            : base(adminBaseEndpoint, handler)
        {

        }

        /// <summary>
        /// List items matching the given query
        /// </summary>
        public Task<AuthInfo> LoginAsync(UserLogin login)
        {
            var requestUri = this.CreateRequestUri(RelativePaths.Login);
            return SendAsync<UserLogin, AuthInfo>(requestUri, HttpMethod.Post, login);
        }
    }
}
