using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OpenDataStorage.Startup))]
namespace OpenDataStorage
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
