using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MarketplaceWeb.Models;
using Omu.ValueInjecter;

using serviceModel = VirtoCommerce.ApiClient.DataContracts.Security;

namespace MarketplaceWeb.Converters
{
    public static class UserConverters
    {
        public static serviceModel.ApplicationUser ToServiceModel(this ApplicationUser user)
        {
            if (user == null) return null;
            var retVal = (serviceModel.ApplicationUser) new serviceModel.ApplicationUser().InjectFrom(user);
            return retVal;
        }

        public static ApplicationUser ToWebModel(this serviceModel.ApplicationUser user)
        {
            if (user == null) return null;
            var retVal = (ApplicationUser) new ApplicationUser().InjectFrom(user);
            return retVal;
        }
    }
}