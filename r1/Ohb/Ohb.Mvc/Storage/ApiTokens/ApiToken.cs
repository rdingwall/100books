using System;

namespace Ohb.Mvc.Storage.ApiTokens
{
    public class ApiToken
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}