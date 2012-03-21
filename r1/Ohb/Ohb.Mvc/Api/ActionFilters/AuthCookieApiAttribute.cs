using System;
using System.Linq;
using System.Net;
using System.Web;
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

        public AuthCookieApiAttribute(IUserRepository users)
        {
            if (users == null) throw new ArgumentNullException("users");
            this.users = users;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var controller = actionContext.ControllerContext.Controller as OhbApiController;
            if (controller == null)
                return;

            if (!OhbUserContext.Current.IsAuthenticated)
            {
                if (RequiresAuthorization(actionContext))
                    throw MissingAuthCookieException();

                return;
            }
            // todo: assert api token has not expired

            controller.User = users.GetUser(OhbUserContext.Current.UserId, controller.DocumentSession);
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