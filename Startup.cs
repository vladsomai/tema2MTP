using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Tema2MTP.Startup))]
namespace Tema2MTP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
