using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.ApiClient.DataContracts.Security
{
    public enum RegisterType
    {
        GuestUser,
        RegisteredUser,
        Administrator,
        SiteAdministrator
    }
}
