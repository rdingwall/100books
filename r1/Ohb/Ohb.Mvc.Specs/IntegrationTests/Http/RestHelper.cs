using System;
using System.Net;
using RestSharp;

namespace Ohb.Mvc.Specs.IntegrationTests.Http
{
    public static class RestHelper
    {
        public static HttpStatusCode GetStatusCode(string path, Method method)
        {
            var client = new RestClient("http://localhost/api");
            var request = new RestRequest(path, method);
            var response = client.Execute(request);

            Console.WriteLine(response.Content);

            return response.StatusCode;
        }
    }
}