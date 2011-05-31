using System.Web.Mvc;
using System.Web.Routing;

namespace Librarian.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(RouteNames.Home, "", new { controller = "Home", action = "Index" } );
            routes.MapRoute(RouteNames.Login, "login", new { controller = "Security", action = "Login" });
            routes.MapRoute(RouteNames.Logout, "logout", new { controller = "Security", action = "Logout" });
            routes.MapRoute(RouteNames.ListProjects, "projects", new { controller = "Project", action = "ListProjects" });
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}