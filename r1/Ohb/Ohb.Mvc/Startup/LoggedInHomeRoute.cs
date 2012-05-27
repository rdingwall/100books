using System;
using System.Web;
using System.Web.Routing;
using Ohb.Mvc.AuthCookies;

namespace Ohb.Mvc.Startup
{
    public class LoggedInHomeRoute : Route
    {
        readonly ICurrentUserContextProvider provider;

        public LoggedInHomeRoute(string url, IRouteHandler routeHandler, ICurrentUserContextProvider provider)
            : base(url, routeHandler)
        {
            if (provider == null) throw new ArgumentNullException("provider");
            this.provider = provider;
        }

        public LoggedInHomeRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler, ICurrentUserContextProvider provider) : base(url, defaults, constraints, dataTokens, routeHandler)
        {
            if (provider == null) throw new ArgumentNullException("provider");
            this.provider = provider;
        }

        public LoggedInHomeRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler, ICurrentUserContextProvider provider) : base(url, defaults, constraints, routeHandler)
        {
            if (provider == null) throw new ArgumentNullException("provider");
            this.provider = provider;
        }

        public LoggedInHomeRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler, ICurrentUserContextProvider provider) : base(url, defaults, routeHandler)
        {
            if (provider == null) throw new ArgumentNullException("provider");
            this.provider = provider;
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var routeData = base.GetRouteData(httpContext);

            var context = provider.GetCurrentUser();
            if (!context.IsAuthenticated)
                return routeData;

            if (routeData == null)
                return routeData; // ?? not sure how this is possible but whatevs

            routeData.Values["Controller"] = "LoggedIn";

            return routeData;
        }
    }
}