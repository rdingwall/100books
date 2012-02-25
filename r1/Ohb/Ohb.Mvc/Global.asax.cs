﻿using System.Web;
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
            filters.Add(new RavenDbAttribute());
        }

        public void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Search",
                            "Search/{query}", // URL with parameters
                            new { controller = "Search", action = "Search" });

            routes.MapRoute("BooksApi",
                            "api/books/{id}", // URL with parameters
                            new { controller = "BooksApi", action = "Get" });

            routes.MapRoute("BooksApi2",
                            "api/books", // URL with parameters
                            new { controller = "BooksApi", action = "Get" });

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

        protected void Application_Start()
        {
            Bootstrapper.With.Windsor().And.AutoMapper().Start();
            
            container = (IWindsorContainer)Bootstrapper.Container;

            DependencyResolver.SetResolver(new WindsorDependencyResolver(container));

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