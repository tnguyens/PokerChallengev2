using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PokerApp.Startup))]
namespace PokerApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
