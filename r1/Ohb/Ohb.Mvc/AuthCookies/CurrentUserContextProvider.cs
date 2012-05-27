using System;
using System.Web;

namespace Ohb.Mvc.AuthCookies
{
    public interface ICurrentUserContextProvider
    {
        OhbUserContext GetCurrentUser();
    }

    public class CurrentUserContextProvider : ICurrentUserContextProvider
    {
        public static readonly string CacheKey = typeof (OhbUserContext).Name;
        readonly IOhbUserContextFactory factory;

        public CurrentUserContextProvider(IOhbUserContextFactory factory)
        {
            if (factory == null) throw new ArgumentNullException("factory");
            this.factory = factory;
        }

        public Func<HttpContextBase> GetHttpContextCurrent = 
            () => new HttpContextWrapper(HttpContext.Current);

        public OhbUserContext GetCurrentUser()
        {
            var httpContext = GetHttpContextCurrent();

            if (httpContext.Items.Contains(CacheKey))
                return (OhbUserContext)httpContext.Items[CacheKey];

            var context = factory.CreateFromAuthCookie(httpContext);
            httpContext.Items[CacheKey] = context;
            return context;
        }
    }
}