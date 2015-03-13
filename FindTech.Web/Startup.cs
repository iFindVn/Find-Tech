using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FindTech.Web.Startup))]
namespace FindTech.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
