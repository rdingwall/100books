using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Ohb.Mvc.Api.Controllers;
using Ohb.Mvc.Storage.Users;
using Raven.Client;

namespace Ohb.Mvc.Api.ActionFilters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ApiAuthorizeAttribute : Attribute {}

    public class ApiAuthorizeHandlerAttribute : ActionFilterAttribute
    {
        readonly IUserRepository users;

        public ApiAuthorizeHandlerAttribute(IUserRepository users)
        {
            if (users == null) throw new ArgumentNullException("users");
            this.users = users;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var controller = actionContext.ControllerContext.Controller as OhbApiController;
            if (controller == null)
                return;

            if (!RequiresAuthorization(actionContext))
                return;

            string apiToken = GetApiTokenCookie();

            // todo: assert api token has not expired

            controller.User = GetUser(apiToken, controller.DocumentSession);
        }

        User GetUser(string apiToken, IDocumentSession session)
        {
            var user = users.GetUserByApiToken(apiToken, session);
            if (user == null)
                throw UnknownUserException();
            return user;
        }

        static string GetApiTokenCookie()
        {
            var cookie = HttpContext.Current.Request.Cookies[OhbCookies.ApiToken];
            if (cookie == null) 
                throw MissingApiTokenException();

            var apiToken = cookie.Value;
            if (String.IsNullOrWhiteSpace(apiToken))
                throw MissingApiTokenException();

            return apiToken;
        }

        static Exception MissingApiTokenException()
        {
            return new HttpResponseException(
                String.Format("This API method requires authentication. Please provide the {0} cookie.", OhbCookies.ApiToken),
                HttpStatusCode.Unauthorized);
        }

        static Exception UnknownUserException()
        {
            return new HttpResponseException(
                String.Format("Unrecognized {0} value.", OhbCookies.ApiToken),
                HttpStatusCode.Unauthorized);
        }

        static bool RequiresAuthorization(HttpActionContext actionContext)
        {
            return actionContext.ActionDescriptor.GetCustomAttributes<ApiAuthorizeAttribute>().Any();
        }
    }
}