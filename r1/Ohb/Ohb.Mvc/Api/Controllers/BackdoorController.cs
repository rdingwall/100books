using System;
using System.Net;
using System.Web;
using System.Web.Http;
using Ohb.Mvc.Api.Models;
using Ohb.Mvc.AuthCookies;
using Ohb.Mvc.Storage.Users;

namespace Ohb.Mvc.Api.Controllers
{
    // NOT FOR PUBLIC USE. This is a harness for testing only. If it gets 
    // deployed, and someone finds it, we are totally fucked
    public class BackdoorController : OhbApiController
    {
        readonly IUserRepository users;
        readonly IAuthCookieFactory authCookieFactory;
        static readonly Random random = new Random();

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
        public dynamic CreateUser(string name, string imageUrl, bool setAuthCookie)
        {
            var user = new User
                            {
                                FacebookId = random.NextNonNegativeLong(),
                                Name = name ?? "TestUser-" + DateTime.Now.Ticks,
                                ProfilePictureUrl = imageUrl
                            };

            users.AddUser(user, DocumentSession);

            if (setAuthCookie)
            {
                var cookie = authCookieFactory.CreateAuthCookie(user);

                // Yes, this will only work in IIS
                HttpContext.Current.Response.Cookies.Add(cookie);
            }

            return new BackdoorCreateUserResponse { UserId = user.Id };
        }

    }
}