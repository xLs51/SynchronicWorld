using System.Web.Mvc;
using System.Web.Routing;
using SynchronicWorld.Models;

namespace SynchronicWorld
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            // Initialize the db
            SynchronicWorldInitializer.Initialize();
        }
    }
}
