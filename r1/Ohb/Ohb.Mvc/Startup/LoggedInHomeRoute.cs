using System.Web;
using System.Web.Routing;
using Facebook.Web;

namespace Ohb.Mvc.Startup
{
    public class LoggedInHomeRoute : Route
    {
        public LoggedInHomeRoute(string url, IRouteHandler routeHandler)
            : base(url, routeHandler)
        {
        }

        public LoggedInHomeRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler) : base(url, defaults, constraints, dataTokens, routeHandler)
        {
        }

        public LoggedInHomeRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler) : base(url, defaults, constraints, routeHandler)
        {
        }

        public LoggedInHomeRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler) : base(url, defaults, routeHandler)
        {
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var routeData = base.GetRouteData(httpContext);

            if (!FacebookWebContext.Current.IsAuthenticated())
                return routeData;

            if (routeData == null)
                return routeData; // ?? not sure how this is possible but whatevs

            routeData.Values["Controller"] = "Profile";

            return routeData;
        }
    }
}