using System;

namespace Ohb.Mvc.Authentication
{
    public class AuthCookieContext
    {
        public string UserId { get; set; }
        public DateTime ExpirationTime { get; set; }

        public bool IsExpired()
        {
            return ExpirationTime < DateTime.UtcNow;
        }
    }
}