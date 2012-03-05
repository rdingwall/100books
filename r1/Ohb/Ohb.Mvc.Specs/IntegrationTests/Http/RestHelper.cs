using System;
using System.Linq;
using System.Net;
using Ohb.Mvc.AuthCookies;
using Ohb.Mvc.Storage;
using Ohb.Mvc.Storage.Users;
using Raven.Client.Document;
using RestSharp;

namespace Ohb.Mvc.Specs.IntegrationTests.Http
{
    public static class RestHelper
    {
        public static HttpStatusCode GetStatusCode(string path, Method method)
        {
            var client = new RestClient("http://localhost/api/v1");
            var request = new RestRequest(path, method);
            var response = client.Execute(request);

            Console.WriteLine(response.Content);

            return response.StatusCode;
        }

        public static string GetRandomUserAuthCookie()
        {
            using (var documentStore = new DocumentStore
            {
                Url = "http://localhost:8080",
                DefaultDatabase = "Ohb"
            })
            {
                documentStore.Initialize();
                using (var documentSession = documentStore.OpenSession())
                {
                    var userId = documentSession.Query<User>().First().Id;

                    return GetAuthCookie(userId);
                }
            }
        }

        public static string GetAuthCookie(string userId)
        {
            using (var encoder = new AuthCookieEncoder(secretKey: "vipbOO5m4RGVGBuUSCQBmw=="))
            {
                var factory = new AuthCookieFactory(encoder);
                var cookie = factory.CreateAuthCookie(new User {Id = userId});
                return cookie.Value;
            }
        }

    }
}