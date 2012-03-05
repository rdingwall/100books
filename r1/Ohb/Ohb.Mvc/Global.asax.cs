using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Bootstrap;
using Bootstrap.AutoMapper;
using Bootstrap.Windsor;
using Castle.Windsor;
using Ohb.Mvc.ActionFilters;
using Ohb.Mvc.Api.ActionFilters;
using Ohb.Mvc.Startup;

namespace Ohb.Mvc
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        private IWindsorContainer container;

        public static void RegisterGlobalFilters(GlobalFilterCollection filters, 
            IWindsorContainer container)
        {
            filters.Add(container.Resolve<OhbHandleErrorAttribute>());
            filters.Add(container.Resolve<RavenDbAttribute>());
            filters.Add(container.Resolve<CurrentUserAttribute>());
            filters.Add(container.Resolve<AuthCookieAttribute>());
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
                           new MvcRouteHandler()));
        }

        static void RegisterApiRoutes(RouteCollection routes)
        {
            routes.MapHttpRoute("BookStatusesById",
                                routeTemplate: "api/v1/books/{volumeIds}/statuses", // URL with parameters
                                defaults: new
                                              {
                                                  controller = "BookStatuses",
                                                  volumeIds = RouteParameter.Optional
                                              });

            routes.MapHttpRoute("VolumeIdBasedApi",
                                routeTemplate: "api/v1/{controller}/{volumeId}",
                                defaults: new { volumeId = RouteParameter.Optional });

            routes.MapHttpRoute("DefaultApi",
                                routeTemplate: "api/v1/{controller}/{id}", // URL with parameters
                                defaults: new {id = RouteParameter.Optional});
        }

        protected void Application_Start()
        {
            Bootstrapper.With.Windsor().And.AutoMapper().Start();
            
            container = (IWindsorContainer)Bootstrapper.Container;

            var resolver = new WindsorResolver(container);

            // Web API
            RegisterApiStuff(resolver, container);

            DependencyResolver.SetResolver(resolver);

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters, container);
            RegisterRoutes(RouteTable.Routes);
        }

        static void RegisterApiStuff(WindsorResolver resolver, IWindsorContainer container)
        {
            var config = GlobalConfiguration.Configuration;
            config.ServiceResolver.SetResolver(resolver);
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Filters.Add(container.Resolve<RavenDbApiAttribute>());
            config.Filters.Add(container.Resolve<RequiresAuthCookieApiAttribute>());
            config.Filters.Add(container.Resolve<OhbErrorHandlerApiAttribute>());
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