﻿using System;
using System.Net;
using System.Web;
using System.Web.Http;

namespace Ohb.Mvc.Authentication
{
    public interface ICurrentUserInfoFactory : IDisposable
    {
        CurrentUserInfo CreateFromAuthCookie(HttpContextBase httpContext);
    }

    public class CurrentUserInfoFactory : ICurrentUserInfoFactory
    {
        IAuthCookieEncoder encoder;
        static readonly CurrentUserInfo unknownUser = 
            new CurrentUserInfo { IsAuthenticated = false };

        public CurrentUserInfoFactory(IAuthCookieEncoder encoder)
        {
            if (encoder == null) throw new ArgumentNullException("encoder");
            this.encoder = encoder;
        }

        public CurrentUserInfo CreateFromAuthCookie(HttpContextBase httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException("httpContext");

            var cookie = httpContext.Request.Cookies[OhbCookies.AuthCookie];

            if (cookie == null)
                return unknownUser;

            var cookieContext = GetAuthCookieContext(cookie);

            if (cookieContext.IsExpired())
                return unknownUser;

            return new CurrentUserInfo
                       {
                           IsAuthenticated = true,
                           UserId = cookieContext.UserId,
                           AuthCookie = cookieContext // null if not logged in
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

        ~CurrentUserInfoFactory()
        {
            Dispose(false);
        }

        bool disposed;
    }
}