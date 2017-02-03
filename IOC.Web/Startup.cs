using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IOC.Web.Startup))]
namespace IOC.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
