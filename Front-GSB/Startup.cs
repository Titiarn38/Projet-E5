using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Front_GSB.Startup))]
namespace Front_GSB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
