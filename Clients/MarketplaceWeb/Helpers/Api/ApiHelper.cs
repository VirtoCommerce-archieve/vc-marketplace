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
        public VirtoCommerceMerchandisingModuleWebModelCategory GetCategory(MerchandisingModuleApi api, string storeName, string locale, string code)
        {
            VirtoCommerceMerchandisingModuleWebModelCategory retVal;
            try
            {
                retVal = api.MerchandisingModuleCategoryGetCategoryByCode(storeName, code, locale);
            }
            catch(ApiException)
            {
                retVal = null;
            }

            return retVal;
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