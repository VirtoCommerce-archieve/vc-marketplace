using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client.Client;

namespace MarketplaceWeb.Helpers
{
    public class ApiHelper
    {
        public VirtoCommerceCatalogModuleWebModelCategory GetCategory(SearchModuleApi api, string storeName, string locale, string code)
        {
            //VirtoCommerceCatalogModuleWebModelCategory retVal;
            //try
            //{
            //    var categories = api.SearchModuleSearch(criteriaCode: code, );
            //}
            //catch(ApiException)
            //{
            //    retVal = null;
            //}

            return null;
        }

        public VirtoCommerceCatalogModuleWebModelProduct GetProduct(CommerceCoreModuleApi api, CatalogModuleApi catalogApi, string code)
        {
            VirtoCommerceCatalogModuleWebModelProduct retVal;

            try
            {
                var seoInfos = api.CommerceGetSeoInfoBySlug(code);
                if(seoInfos != null && seoInfos.Count > 0)
                {
                    retVal = catalogApi.CatalogModuleProductsGet(seoInfos.FirstOrDefault().ObjectId);
                }
                else
                {
                    retVal = null;
                }
            }
            catch (ApiException)
            {
                retVal = null;
            }

            return retVal;
        }

        public VirtoCommerceCustomerModuleWebModelContact GetContact(CustomerManagementModuleApi api, string id)
        {
            VirtoCommerceCustomerModuleWebModelContact retVal;

            try
            {
                retVal = api.CustomerModuleGetContactById(id);
            }
            catch(ApiException)
            {
                retVal = null;
            }

            return retVal;
        }
    }
}