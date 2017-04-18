using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TestOrdersWebProject.Startup))]
namespace TestOrdersWebProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
