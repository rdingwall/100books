using System;
using System.Web;
using System.Web.Routing;
using Ohb.Mvc.Services;

namespace Ohb.Mvc.Startup
{
    public class LoggedInHomeRoute : Route
    {
        private readonly IUserContextFactory userContextFactory;

        public LoggedInHomeRoute(IUserContextFactory userContextFactory,
            string url, IRouteHandler routeHandler)
            : base(url, routeHandler)
        {
            if (userContextFactory == null) throw new ArgumentNullException("userContextFactory");
            this.userContextFactory = userContextFactory;
        }

        public LoggedInHomeRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler, IUserContextFactory userContextFactory) : base(url, defaults, constraints, dataTokens, routeHandler)
        {
            this.userContextFactory = userContextFactory;
        }

        public LoggedInHomeRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler, IUserContextFactory userContextFactory) : base(url, defaults, constraints, routeHandler)
        {
            this.userContextFactory = userContextFactory;
        }

        public LoggedInHomeRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler, IUserContextFactory userContextFactory) : base(url, defaults, routeHandler)
        {
            this.userContextFactory = userContextFactory;
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var routeData = base.GetRouteData(httpContext);

            var context = userContextFactory.GetCurrentContext();

            if (!context.IsLoggedIn)
                return routeData;

            routeData.Values["Controller"] = "Profile";

            return routeData;
        }
    }
}