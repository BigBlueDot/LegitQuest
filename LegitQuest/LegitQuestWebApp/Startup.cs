using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LegitQuestWebApp.Startup))]
namespace LegitQuestWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
