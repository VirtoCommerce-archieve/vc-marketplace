using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MarketplaceWeb.Startup))]
namespace MarketplaceWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
