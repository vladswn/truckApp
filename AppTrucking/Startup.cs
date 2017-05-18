using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AppTrucking.Startup))]
namespace AppTrucking
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
