using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CASMUL.Startup))]
namespace CASMUL
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
