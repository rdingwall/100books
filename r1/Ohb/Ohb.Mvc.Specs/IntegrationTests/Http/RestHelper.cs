using System;
using System.Linq;
using System.Net;
using Ohb.Mvc.Storage;
using Ohb.Mvc.Storage.ApiTokens;
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

        public static string GenerateNewApiToken(string userId = null)
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
                    if (userId == null)
                        userId = documentSession.Query<User>().First().Id;

                    var tokenFactory = new ApiTokenFactory(
                        new CryptoTokenGenerator(), new RavenUniqueInserter());

                    return tokenFactory.CreateApiToken(userId, documentSession).Token;
                }
            }
        }
    }
}