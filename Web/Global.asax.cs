using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Librarian.Logic.TinyPM;

namespace Librarian.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
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
            routes.MapRoute(RouteNames.Backlog,
                "projects/{projectId}/backlog",
                new { controller = "Story", action = "Backlog" });
            routes.MapRoute(RouteNames.StorySetColor,
                "projects/{projectId}/backlog/{storyId}/set-color",
                new { controller = "Story", action = "SetColor" });
            routes.MapRoute(RouteNames.StorySetPosition,
                "projects/{projectId}/backlog/{storyId}/set-position",
                new { controller = "Story", action = "SetPosition" });
        }

        protected void Application_Start()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterAssemblyTypes(typeof(MvcApplication).Assembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(ApiClient).Assembly).AsImplementedInterfaces();
            builder.Register<HttpContextBase>(_ => new HttpContextWrapper(HttpContext.Current));

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}