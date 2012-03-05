using System;
using System.Linq;
using System.Net;
using Ohb.Mvc.AuthCookies;
using Ohb.Mvc.Storage;
using Ohb.Mvc.Storage.Users;
using Raven.Client;
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
            using (var documentSession = OpenLiveRavenSession())
            {
                var userId = documentSession.Query<User>().First().Id;

                return GetAuthCookie(userId);
            }
        }

        public static string GetAuthCookie(string userId)
        {
            using (var encoder = new AuthCookieEncoder(secretKey: "vipbOO5m4RGVGBuUSCQBmw=="))
            {
                var factory = new AuthCookieFactory(encoder);
                var cookie = factory.CreateAuthCookie(new User {Id = userId});
                Console.WriteLine("Using auth cookie: {0} (expires {1})", cookie.Value, cookie.Expires);
                return cookie.Value;
            }
        }

        public static void MarkBookAsRead()
        {
            
        }

        public static void MarkBookAsRead(string volumeId, string authCookie)
        {
            var client = new RestClient("http://localhost/api/v1");
            var request = new RestRequest("previousreads")
            {
                Method = Method.POST,
                RequestFormat = DataFormat.Json
            };
            request.AddCookie(OhbCookies.AuthCookie, authCookie);
            request.AddBody(new { volumeId = volumeId });
            var response = client.Execute(request);

            if (response.ErrorException != null)
                throw new Exception("Couldn't mark book as read for test.", response.ErrorException);
        }

        public static void WaitForNonStaleResults<T>()
        {
            using (var documentSession = OpenLiveRavenSession())
            {
                documentSession.Query<T>()
                    .Customize(a => a.WaitForNonStaleResults())
                    .Any();
            }
        }

        static readonly Lazy<IDocumentStore> liveDocumentStore =
            new Lazy<IDocumentStore>(() =>
                                         {
                                             var ds = new DocumentStore
                                                 {
                                                     Url =
                                                         "http://localhost:8080",
                                                     DefaultDatabase = "Ohb"
                                                 };

                                             ds.Initialize();
                                             return ds;
                                         });

        public static IDocumentStore LiveRavenDocumentStore
        {
            get { return liveDocumentStore.Value; }
        }

        public static IDocumentSession OpenLiveRavenSession()
        {
            return LiveRavenDocumentStore.OpenSession();
        }
    }
}