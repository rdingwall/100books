using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Machine.Specifications;
using Ohb.Mvc.Api.Models;
using Ohb.Mvc.AuthCookies;
using Ohb.Mvc.Storage.Books;
using Ohb.Mvc.Storage.PreviousReads;
using RestSharp;
using RestSharp.Deserializers;

namespace Ohb.Mvc.Specs.IntegrationTests.Http
{
    public class ApiClient
    {
        readonly RestClient client;
        readonly RestClient dynamicClient;
        public string BaseUrl { get; set; }
        public string AuthCookie { get; set; }

        public string UserId
        {
            get
            {
                if (String.IsNullOrWhiteSpace(AuthCookie))
                    return null;

                using (var encoder = new AuthCookieEncoder(AuthCookieSecretKey.Value))
                {
                    AuthCookieContext context;
                    if (!encoder.TryDecode(AuthCookie, out context))
                        return null;

                    return context.UserId;
                }
            }
        }

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
            Console.WriteLine("{0} {1} {2} {3}",
                              (int) response.StatusCode,
                              response.StatusCode,
                              response.Request == null ? "??" : response.Request.Method.ToString(),
                              response.ResponseUri);

            if (response.ErrorException != null)
            {
                Console.WriteLine();
                Console.WriteLine("Response error:");
                Console.WriteLine(response.ErrorException.ToString());
            }


            Console.WriteLine();
            Console.WriteLine("Response headers:");


            foreach (var header in response.Headers)
                Console.WriteLine("   {0} = {1}", header.Name, header.Value);

            Console.WriteLine();
            Console.WriteLine("Response body:");

            var raw = Encoding.UTF8.GetString(response.RawBytes ?? new byte[0]);
            if (response.ContentType.Contains("application/json"))
                raw = new JsonFormatter().PrettyPrint(raw);
            Console.WriteLine(raw);

            Console.WriteLine(new string('-', 80));
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

        public void AssertUnauthorized(string path)
        {
            AssertUnauthorized(Method.GET, path);
        }

        public void AssertUnauthorized(Method method, string path)
        {
            var request = new RestRequest(path) { Method = method };
            var response = client.Execute(request);
            Log(response);
            response.StatusCode.ShouldEqual(HttpStatusCode.Unauthorized);
        }

        public RestResponse<BookModel> GetBook(string googleVolumeId)
        {
            var request = new RestRequest(String.Format("books/{0}", googleVolumeId));
            return Log(client.Execute<BookModel>(request));
        }

        public RestResponse MarkBookAsRead(string googleVolumeId)
        {
            var request = new RestRequest("previousreads")
                              {
                                  Method = Method.PUT,
                                  RequestFormat = DataFormat.Json
                              };

            Authorize(request);

            if (!String.IsNullOrWhiteSpace(googleVolumeId))
                request.AddBody(new {volumeId = googleVolumeId});

            return Log(client.Execute(request));
        }

        void Authorize(IRestRequest request)
        {
            if (String.IsNullOrWhiteSpace(AuthCookie))
                return;

            request.AddCookie(OhbCookies.AuthCookie, AuthCookie);
        }

        public RestResponse<List<PreviousReadModel>> GetPreviousReads()
        {
            var request = new RestRequest("previousreads");
            Authorize(request);
            return Log(client.Execute<List<PreviousReadModel>>(request));
        }

        public RestResponse<List<BookStatus>> GetBookStatuses(params string[] bookIds)
        {
            var ids = String.Join(",", bookIds);
            var request = new RestRequest(String.Format("books/{0}/statuses", ids));
            Authorize(request);
            return Log(client.Execute<List<BookStatus>>(request));
        }
    }
}