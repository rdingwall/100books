using System;
using System.Web;
using System.Web.Routing;
using Ohb.Mvc.AuthCookies;

namespace Ohb.Mvc.Startup
{
    public class LoggedInHomeRoute : Route
    {
        readonly ICurrentUserInfoProvider provider;

        public LoggedInHomeRoute(string url, IRouteHandler routeHandler, ICurrentUserInfoProvider provider)
            : base(url, routeHandler)
        {
            if (provider == null) throw new ArgumentNullException("provider");
            this.provider = provider;
        }

        public LoggedInHomeRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler, ICurrentUserInfoProvider provider) : base(url, defaults, constraints, dataTokens, routeHandler)
        {
            if (provider == null) throw new ArgumentNullException("provider");
            this.provider = provider;
        }

        public LoggedInHomeRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler, ICurrentUserInfoProvider provider) : base(url, defaults, constraints, routeHandler)
        {
            if (provider == null) throw new ArgumentNullException("provider");
            this.provider = provider;
        }

        public LoggedInHomeRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler, ICurrentUserInfoProvider provider) : base(url, defaults, routeHandler)
        {
            if (provider == null) throw new ArgumentNullException("provider");
            this.provider = provider;
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var routeData = base.GetRouteData(httpContext);

            var currentUserInfo = provider.GetCurrentUserInfo();
            if (!currentUserInfo.IsAuthenticated)
                return routeData;

            if (routeData == null)
                return routeData; // ?? not sure how this is possible but whatevs

            routeData.Values["Controller"] = "LoggedIn";

            return routeData;
        }
    }
}