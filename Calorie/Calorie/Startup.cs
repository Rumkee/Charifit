using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Calorie.Startup))]
namespace Calorie
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
