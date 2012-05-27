using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Ohb.Mvc.Api.Controllers;
using Ohb.Mvc.AuthCookies;
using Ohb.Mvc.Storage.Users;

namespace Ohb.Mvc.Api.ActionFilters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RequiresAuthCookieAttribute : Attribute
    {}
    
    // handler
    public class AuthCookieApiAttribute : ActionFilterAttribute
    {
        readonly IUserRepository users;
        readonly ICurrentUserContextProvider provider;

        public AuthCookieApiAttribute(IUserRepository users, ICurrentUserContextProvider provider)
        {
            if (users == null) throw new ArgumentNullException("users");
            if (provider == null) throw new ArgumentNullException("provider");
            this.users = users;
            this.provider = provider;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var controller = actionContext.ControllerContext.Controller as OhbApiController;
            if (controller == null)
                return;

            var context = provider.GetCurrentUser();

            if (!context.IsAuthenticated)
            {
                if (RequiresAuthorization(actionContext))
                    throw MissingAuthCookieException();

                return;
            }
            // todo: assert api token has not expired

            controller.User = users.GetUser(context.UserId, controller.DocumentSession);
        }

        static Exception MissingAuthCookieException()
        {
            return new HttpResponseException(
                String.Format("This API method requires authentication. Please provide the '{0}' auth cookie.", OhbCookies.AuthCookie),
                HttpStatusCode.Unauthorized);
        }

        static bool RequiresAuthorization(HttpActionContext actionContext)
        {
            return actionContext.ActionDescriptor
                .GetCustomAttributes<RequiresAuthCookieAttribute>().Any();
        }
    }
}