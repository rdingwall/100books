#if ENABLE_BACKDOOR_API
using System;
using System.Net;
using System.Web;
using System.Web.Http;
using Ohb.Mvc.Api.ActionFilters;
using Ohb.Mvc.Api.Models;
using Ohb.Mvc.Authentication;
using Ohb.Mvc.Storage.Users;

namespace Ohb.Mvc.Api.Controllers
{
    // NOT FOR PUBLIC USE. This is a harness for testing only. If it gets 
    // deployed, and someone finds it, we are totally fucked
    [FilterIP(AllowedSingleIPs = "localhost, 127.0.0.1")]
    public class BackdoorController : OhbApiController
    {
        readonly IUserRepository users;
        readonly IAuthCookieFactory authCookieFactory;

        public BackdoorController(IUserRepository users, 
            IAuthCookieFactory authCookieFactory)
        {
            if (users == null) throw new ArgumentNullException("users");
            if (authCookieFactory == null) throw new ArgumentNullException("authCookieFactory");
            this.users = users;
            this.authCookieFactory = authCookieFactory;
        }

        // Major security back door!
        public void GetAuthCookie(string userId)
        {
            if (String.IsNullOrWhiteSpace(userId))
                throw new HttpResponseException("Missing 'userId' parameter.", HttpStatusCode.BadRequest);

            var user = users.GetUser(userId, DocumentSession);

            if (user == null)
                throw new HttpResponseException("User not found.", HttpStatusCode.NotFound);

            var cookie = authCookieFactory.CreateAuthCookie(user);

            // Yes, this will only work in IIS
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        [HttpPost]
        public dynamic CreateUser(string displayName, string profileImageUrl, bool? setAuthCookie)
        {
            var userId = "TestUser-" + DateTime.UtcNow.Ticks;

            var user = new User
                            {
                                FacebookId = userId,
                                DisplayName = displayName ?? userId,
                                ProfileImageUrl = profileImageUrl
                            };

            users.AddUser(user, DocumentSession);

            if (setAuthCookie.GetValueOrDefault())
            {
                var cookie = authCookieFactory.CreateAuthCookie(user);

                // Yes, this will only work in IIS
                HttpContext.Current.Response.Cookies.Add(cookie);
            }

            return new BackdoorCreateUserResponse { UserId = user.Id };
        }

    }
}
#endif // ENABLE_BACKDOOR_API