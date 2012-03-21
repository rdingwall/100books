using System;
using System.Net;
using System.Web;
using System.Web.Http;

namespace Ohb.Mvc.AuthCookies
{
    public interface IOhbUserContextFactory : IDisposable
    {
        OhbUserContext CreateFromAuthCookie(HttpContextBase httpContext);
    }

    public class OhbUserContextFactory : IOhbUserContextFactory
    {
        IAuthCookieEncoder encoder;

        public OhbUserContextFactory(IAuthCookieEncoder encoder)
        {
            if (encoder == null) throw new ArgumentNullException("encoder");
            this.encoder = encoder;
        }

        public OhbUserContext CreateFromAuthCookie(HttpContextBase httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException("httpContext");

            var cookie = httpContext.Request.Cookies[OhbCookies.AuthCookie];

            if (cookie == null)
                return new OhbUserContext {IsAuthenticated = false};

            var cookieContext = GetAuthCookieContext(cookie);

            return new OhbUserContext
                       {
                           IsAuthenticated = true,
                           UserId = cookieContext.UserId,
                       };
        }

        AuthCookieContext GetAuthCookieContext(HttpCookie cookie)
        {
            AuthCookieContext context;
            if (!encoder.TryDecode(cookie.Value, out context))
                throw BadAuthCookieException();
            return context;
        }

        static Exception BadAuthCookieException()
        {
            return new HttpResponseException("Invalid auth cookie.", HttpStatusCode.Unauthorized);
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

        ~OhbUserContextFactory()
        {
            Dispose(false);
        }

        bool disposed;
    }
}