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
    public class AuthCookieApiAttribute : ActionFilterAttribute, IDisposable
    {
        readonly IUserRepository users;
        IAuthCookieEncoder encoder;

        public AuthCookieApiAttribute(IUserRepository users, 
            IAuthCookieEncoder encoder)
        {
            if (users == null) throw new ArgumentNullException("users");
            if (encoder == null) throw new ArgumentNullException("encoder");
            this.users = users;
            this.encoder = encoder;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var controller = actionContext.ControllerContext.Controller as OhbApiController;
            if (controller == null)
                return;

            var cookie = HttpContext.Current.Request.Cookies[OhbCookies.AuthCookie];

            if (cookie == null)
            {
                if (RequiresAuthorization(actionContext))
                    throw MissingAuthCookieException();

                return;
            }

            AuthCookieContext context = GetAuthCookieContext(cookie);

            // todo: assert api token has not expired

            controller.User = users.GetUser(context.UserId, controller.DocumentSession);
        }

        AuthCookieContext GetAuthCookieContext(HttpCookie cookie)
        {
            AuthCookieContext context;
            if (!encoder.TryDecode(cookie.Value, out context))
                throw BadAuthCookieException();
            return context;
        }

        static Exception MissingAuthCookieException()
        {
            return new HttpResponseException(
                String.Format("This API method requires authentication. Please provide the '{0}' auth cookie.", OhbCookies.AuthCookie),
                HttpStatusCode.Unauthorized);
        }

        static Exception BadAuthCookieException()
        {
            return new HttpResponseException("Invalid auth cookie.", HttpStatusCode.Unauthorized);
        }

        static bool RequiresAuthorization(HttpActionContext actionContext)
        {
            return actionContext.ActionDescriptor
                .GetCustomAttributes<RequiresAuthCookieAttribute>().Any();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (encoder != null)
                    {
                        encoder.Dispose();
                        encoder = null;
                    }
                }

                disposed = true;
            }
        }

        ~AuthCookieApiAttribute()
        {
            Dispose(false);
        }

        bool disposed;
    }
}