using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UnseentalentsApp.Startup))]
namespace UnseentalentsApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
