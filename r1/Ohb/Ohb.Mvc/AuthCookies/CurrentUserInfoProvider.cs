using System;
using System.Web;

namespace Ohb.Mvc.AuthCookies
{
    public interface ICurrentUserInfoProvider
    {
        CurrentUserInfo GetCurrentUserInfo();
    }

    public class CurrentUserInfoProvider : ICurrentUserInfoProvider
    {
        public static readonly string CacheKey = typeof (CurrentUserInfo).Name;
        readonly ICurrentUserInfoFactory factory;

        public CurrentUserInfoProvider(ICurrentUserInfoFactory factory)
        {
            if (factory == null) throw new ArgumentNullException("factory");
            this.factory = factory;
        }

        public Func<HttpContextBase> GetHttpContextCurrent = 
            () => new HttpContextWrapper(HttpContext.Current);

        public CurrentUserInfo GetCurrentUserInfo()
        {
            var httpContext = GetHttpContextCurrent();

            if (httpContext.Items.Contains(CacheKey))
                return (CurrentUserInfo)httpContext.Items[CacheKey];

            var currentUserInfo = factory.CreateFromAuthCookie(httpContext);
            httpContext.Items[CacheKey] = currentUserInfo;
            return currentUserInfo;
        }
    }
}