using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Bootstrap;
using Bootstrap.AutoMapper;
using Bootstrap.Windsor;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Elmah.Contrib.Mvc;
using Elmah.Contrib.WebApi;
using Microsoft.Practices.ServiceLocation;
using Ohb.Mvc.ActionFilters;
using Ohb.Mvc.Api;
using Ohb.Mvc.Api.ActionFilters;
using Ohb.Mvc.AuthCookies;
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
            filters.Add(new ElmahHandleErrorAttribute());
            filters.Add(container.Resolve<RavenDbAttribute>());
            filters.Add(container.Resolve<CurrentUserAttribute>());
            filters.Add(container.Resolve<AuthCookieAttribute>());
            filters.Add(new HandleErrorAttribute());
        }

        public void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            RegisterApiRoutes(routes);

            routes.MapRoute("Books",
                            "Books/{id}", // URL with parameters
                            new { controller = "Books", action = "Get" });

            routes.MapRoute("Redirect", "FbLogin",
                            new { controller = "Public", action = "FbLogin" });

            routes.MapRoute("ForceError", "Error",
                           new { controller = "Public", action = "Error" });

            routes.Add(new LoggedInHomeRoute(
                           "{*catchall}",
                           new RouteValueDictionary( // Parameter defaults
                               new {controller = "Public", action = "Index", id = UrlParameter.Optional}),
                           new MvcRouteHandler(),
                           container.Resolve<ICurrentUserContextProvider>()));
        }

        static void RegisterApiRoutes(RouteCollection routes)
        {
#if ENABLE_BACKDOOR_API
            RegisterBackdoorApiRoutes(routes);
#endif // ENABLE_BACKDOOR_API

            routes.MapHttpRoute("BookStatusesById",
                                routeTemplate: "api/v1/books/{volumeIds}/statuses", // URL with parameters
                                defaults: new
                                              {
                                                  controller = "BookStatuses",
                                                  volumeIds = RouteParameter.Optional
                                              });

            routes.MapHttpRoute("MyProfileApi",
                                routeTemplate: "api/v1/profiles/me", // URL with parameters
                                defaults: new
                                {
                                    controller = "Profiles",
                                    action = "GetMe"
                                });

            routes.MapHttpRoute("ProfilesApiWithoutParameter",
                                routeTemplate: "api/v1/profiles/", // URL with parameters
                                defaults: new
                                {
                                    controller = "Profiles",
                                    action = "Get"
                                });

            routes.MapHttpRoute("ProfilesApi",
                                routeTemplate: "api/v1/profiles/{id}", // URL with parameters
                                defaults: new
                                              {
                                                  controller = "Profiles",
                                                  id = RouteParameter.Optional
                                              });

            routes.MapHttpRoute("VolumeIdBasedApi",
                                routeTemplate: "api/v1/{controller}/{volumeId}",
                                defaults: new { volumeId = RouteParameter.Optional });

            routes.MapHttpRoute("DefaultApi",
                                routeTemplate: "api/v1/{controller}/{id}", // URL with parameters
                                defaults: new {id = RouteParameter.Optional});
        }

#if ENABLE_BACKDOOR_API
        static void RegisterBackdoorApiRoutes(RouteCollection routes)
        {
            routes.MapHttpRoute("BackdoorGetAuthCookieApiRoute",
                                routeTemplate: "api/backdoor/getauthcookie",
                                defaults: new {controller = "Backdoor", action = "GetAuthCookie"});

            routes.MapHttpRoute("BackdoorCreateUserApiRoute",
                                routeTemplate: "api/backdoor/createuser",
                                defaults: new
                                              {
                                                  controller = "Backdoor",
                                                  action = "CreateUser"
                                              });
        }
#endif // ENABLE_BACKDOOR_API

        protected void Application_Start()
        {
            Bootstrapper.With.Windsor().And.AutoMapper().Start();
            
            container = (IWindsorContainer)Bootstrapper.Container;

            var resolver = new WindsorServiceLocator(container);

            // Web API
            RegisterApiStuff(resolver, container);

            DependencyResolver.SetResolver(resolver);

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters, container);
            RegisterRoutes(RouteTable.Routes);
        }

        static void RegisterApiStuff(IServiceLocator resolver, IWindsorContainer container)
        {
            var config = GlobalConfiguration.Configuration;
            config.ServiceResolver.SetResolver(resolver);
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            var index = config.Formatters.IndexOf(config.Formatters.JsonFormatter);
            config.Formatters[index] = new JsonCamelCaseFormatter();

            config.Filters.Add(container.Resolve<RavenDbApiAttribute>());
            config.Filters.Add(container.Resolve<AuthCookieApiAttribute>());
            config.Filters.Add(new ElmahHandleErrorApiAttribute());
            config.Filters.Add(container.Resolve<GoogleAnalyticsTrackerApiAttribute>());
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