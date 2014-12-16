﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient.Utilities;

namespace VirtoCommerce.ApiClient.Extensions
{
    using System.Configuration;

    public static class ItemsClientExtension
    {
        public static ItemsClient CreateItemsClient(this CommerceClients source)
        {
            var connectionString = ConnectionHelper.GetConnectionString("vc-commerce-api-mp");
            return CreateItemsClient(source, connectionString);
        }

        public static ItemsClient CreateItemsClient(this CommerceClients source, string serviceUrl)
        {
            var connectionString = serviceUrl;
            var subscriptionKey = ConfigurationManager.AppSettings["vc-marketplace-apikey"];
            var client = new ItemsClient(new Uri(connectionString), new AzureSubscriptionMessageProcessingHandler(subscriptionKey, "secret"));
            return client;
        }
    }
}
