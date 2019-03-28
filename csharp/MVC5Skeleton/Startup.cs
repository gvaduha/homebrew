using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVC5Skeleton.Startup))]
namespace MVC5Skeleton
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
