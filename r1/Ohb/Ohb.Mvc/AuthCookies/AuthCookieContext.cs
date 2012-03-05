using System;

namespace Ohb.Mvc.AuthCookies
{
    public class AuthCookieContext
    {
        public string UserId { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}