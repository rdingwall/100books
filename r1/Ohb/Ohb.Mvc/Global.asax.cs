using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Bootstrap;
using Bootstrap.AutoMapper;
using Bootstrap.Windsor;
using Castle.Windsor;
using Ohb.Mvc.Services;
using Ohb.Mvc.Startup;

namespace Ohb.Mvc
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        private IWindsorContainer container;

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new OhbHandleErrorAttribute());
        }

        public void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Search",
                            "Search/{query}", // URL with parameters
                            new { controller = "Search", action = "Search" });

            RegisterApiRoutes(routes);

            routes.MapRoute("Books",
                            "Books/{id}", // URL with parameters
                            new { controller = "Books", action = "Get" });

            routes.MapRoute("Redirect", "Profile",
                            new { controller = "Profile", action = "Redirect" });

            routes.Add(new LoggedInHomeRoute(
                           "{controller}/{action}/{id}", // URL with parameters
                           new RouteValueDictionary( // Parameter defaults
                               new { controller = "Home", action = "Index", id = UrlParameter.Optional }),
                           new MvcRouteHandler(),
                           container.Resolve<IUserContextFactory>()));
        }

        static void RegisterApiRoutes(RouteCollection routes)
        {
            routes.MapHttpRoute("DefaultApi",
                                routeTemplate: "api/{controller}/{id}", // URL with parameters
                                defaults: new {id = RouteParameter.Optional});
        }

        protected void Application_Start()
        {
            Bootstrapper.With.Windsor().And.AutoMapper().Start();
            
            container = (IWindsorContainer)Bootstrapper.Container;

            var resolver = new WindsorResolver(container);
            GlobalConfiguration.Configuration.ServiceResolver.SetResolver(resolver);
            GlobalConfiguration.Configuration.MessageHandlers.Add(new RavenDbHandler(container));

            DependencyResolver.SetResolver(resolver);

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        public override void Dispose()
        {
            if (container != null)
            {
                container.Dispose();
                container = null;
            }
            base.Dispose();
        }
    }
}