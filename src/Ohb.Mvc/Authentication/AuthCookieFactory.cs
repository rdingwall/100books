using System;
using System.Web;
using Ohb.Mvc.Storage.Users;

namespace Ohb.Mvc.Authentication
{
    public interface IAuthCookieFactory
    {
        HttpCookie CreateAuthCookie(User user);
    }

    public class AuthCookieFactory : IAuthCookieFactory
    {
        public static readonly TimeSpan ExpiryDuration = TimeSpan.FromHours(2);
        readonly IAuthCookieEncoder encoder;

        public AuthCookieFactory(IAuthCookieEncoder encoder)
        {
            if (encoder == null) throw new ArgumentNullException("encoder");
            this.encoder = encoder;
        }

        public HttpCookie CreateAuthCookie(User user)
        {
            if (user == null) throw new ArgumentNullException("user");

            var context = new AuthCookieContext
                              {
                                  ExpirationTime = DateTime.UtcNow.Add(ExpiryDuration),
                                  UserId = user.Id
                              };

            var cookieValue = encoder.Encode(context);

            return new HttpCookie(OhbCookies.AuthCookie, cookieValue)
                       {
                           Expires = context.ExpirationTime
                       };
        }
    }
}