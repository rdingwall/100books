using System;
using System.Net;
using System.Text;
using Machine.Specifications;
using Ohb.Mvc.Storage.Books;
using RestSharp;
using RestSharp.Deserializers;

namespace Ohb.Mvc.Specs.IntegrationTests.Http
{
    public class ApiClient
    {
        readonly RestClient client;
        readonly RestClient dynamicClient;
        public string BaseUrl { get; set; }

        public ApiClient()
        {
            BaseUrl = "http://localhost/api/v1";

            client = new RestClient(BaseUrl);

            // Only get one handler per content type so easiest just to have 2
            // clients
            dynamicClient = new RestClient(BaseUrl);
            dynamicClient.AddHandler("application/json", new DynamicJsonDeserializer());
        }

        static RestResponse Log(RestResponse response)
        {
            DoLog(response);
            return response;
        }

        static void DoLog(RestResponseBase response)
        {
            Console.WriteLine("Headers:");
            Console.WriteLine("------------------------------------------------");

            foreach (var header in response.Headers)
                Console.WriteLine("{0} = {1}", header.Name, header.Value);

            Console.WriteLine();
            Console.WriteLine("Body:");
            Console.WriteLine("------------------------------------------------");

            var raw = Encoding.UTF8.GetString(response.RawBytes);
            if (response.ContentType.Contains("application/json"))
                raw = new JsonFormatter().PrettyPrint(raw);
            Console.WriteLine(raw);

            Console.WriteLine("------------------------------------------------");
        }

        static RestResponse<T> Log<T>(RestResponse<T> response)
        {
            DoLog(response);
            return response;
        }

        public RestResponse<dynamic> Get(string path)
        {
            var request = new RestRequest(path);
            return Log(dynamicClient.Execute<dynamic>(request));
        }

        public RestResponse<dynamic> Post(string path)
        {
            var request = new RestRequest(path) {Method = Method.POST};
            return Log(dynamicClient.Execute<dynamic>(request));
        }

        public void AssertReturns(Method method, string path, HttpStatusCode expectedStatusCode)
        {
            var request = new RestRequest(path) { Method = method };
            var response = client.Execute(request);
            Log(response);
            response.StatusCode.ShouldEqual(expectedStatusCode);
        }

        public void AssertMethodNotAllowed(Method method, string path)
        {
            var request = new RestRequest(path) { Method = method };
            var response = client.Execute(request);
            Log(response);
            response.StatusCode.ShouldEqual(HttpStatusCode.MethodNotAllowed);
        }

        public RestResponse<Book> GetBook(string googleVolumeId)
        {
            var request = new RestRequest(String.Format("books/{0}", googleVolumeId));
            return Log(client.Execute<Book>(request));
        }
    }
}