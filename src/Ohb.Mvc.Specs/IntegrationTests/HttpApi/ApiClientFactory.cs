using System;
using System.Web;
using Ohb.Mvc.Authentication;
using Ohb.Mvc.Storage.Users;
using System.Linq;

namespace Ohb.Mvc.Specs.IntegrationTests.HttpApi
{
    public class ApiClientFactory
    {
        static readonly Random random = new Random();

        public static ApiClient PublicClient()
        {
            return new ApiClient();
        }

        static User CreateNewUser()
        {
            var displayName = "TestUser-" + DateTime.UtcNow.Ticks;

            var users = new UserRepository();
            using (var session = LiveRavenDb.OpenSession())
            {
                users.AddUser(new User
                                  {
                                      DisplayName = displayName,
                                      FacebookId = random.NextNonNegativeLong().ToString()
                                  }, session);
                var user = session.Query<User>()
                    .Customize(a => a.WaitForNonStaleResults())
                    .Single(u => u.DisplayName == displayName);

                return user;
            }
        }

        static HttpCookie GetAuthCookie(string userId)
        {
            using (var encoder = new AuthCookieEncoder(secretKey: AuthCookieSecretKey.Value))
            {
                var factory = new AuthCookieFactory(encoder);
                var cookie = factory.CreateAuthCookie(new User { Id = userId });
                return cookie;
            }
        }

        public static ApiClient NewUser()
        {
            var user = CreateNewUser();
            var cookie = GetAuthCookie(user.Id);

            Console.WriteLine("user id: {0} display name: {1}", user.Id, user.DisplayName);
            Console.WriteLine("auth cookie: {0} (expires {1})", cookie.Value, cookie.Expires);

            return new ApiClient {AuthCookie = cookie.Value, User = user};
        }

        public static ApiClient Anonymous()
        {
            return new ApiClient();
        }
    }
}