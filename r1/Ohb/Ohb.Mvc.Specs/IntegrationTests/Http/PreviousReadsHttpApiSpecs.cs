using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Machine.Specifications;
using RestSharp;
using RestSharp.Deserializers;

namespace Ohb.Mvc.Specs.IntegrationTests.Http
{
    class PreviousReadsHttpApiSpecs
    {
        [Subject("api/v1/previousreads GET")]
        public class when_retrieving_previous_reads
        {
            public class when_there_was_no_api_token
            {
                Establish context =
                    () =>
                    {
                        client = new RestClient("http://localhost/api/v1");
                        request = new RestRequest("previousreads")
                        {
                            Method = Method.GET,
                            RequestFormat = DataFormat.Json
                        };
                    };

                Because of = () => response = client.Execute(request);

                It should_return_http_401_unauthorized =
                    () => response.StatusCode.ShouldEqual(HttpStatusCode.Unauthorized);

                static RestClient client;
                static RestRequest request;
                static RestResponse response;
            }
        }

        [Subject("api/v1/previousreads POST")]
        public class when_marking_a_book_as_previously_read
        {
            public class when_it_is_a_valid_request
            {
                Establish context =
                    () =>
                        {
                            apiToken = RestHelper.GenerateNewApiToken();

                            client = new RestClient("http://localhost/api/v1");
                            request = new RestRequest("previousreads")
                                          {
                                              Method = Method.POST,
                                              RequestFormat = DataFormat.Json
                                          };
                            request.AddCookie(OhbCookies.ApiToken, apiToken);

                            request.AddBody(new { volumeId = "4YydO00I9JYC" });
                        };

                Because of = () => response = client.Execute<dynamic>(request);

                It should_return_http_200_ok = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);

                It should_return_the_full_book_details =
                    () =>
                        {
                            request = new RestRequest("previousreads");
                            request.AddCookie(OhbCookies.ApiToken, apiToken);
                            client.AddHandler("application/json", new DynamicJsonDeserializer());
                            var response = client.Execute<dynamic>(request);

                            response.ContentType.ShouldEqual("application/json; charset=utf-8");
                            ((object) response.Data).ShouldBe(typeof (IEnumerable));

                            var books = (IEnumerable<dynamic>) response.Data;

                            books.ShouldNotBeEmpty();
                            books.Select(b => (string)b.StaticInfo.Id.Value).ShouldContain("4YydO00I9JYC");
                            books.Select(b => (string)b.StaticInfo.Title.Value).ShouldContain("The Google story");
                        };

                static RestResponse<dynamic> response;
                static RestClient client;
                static RestRequest request;
                static string apiToken;
            }

            public class when_there_was_no_api_token
            {
                Establish context =
                    () =>
                    {
                        client = new RestClient("http://localhost/api/v1");
                        request = new RestRequest("previousreads")
                        {
                            Method = Method.POST,
                            RequestFormat = DataFormat.Json
                        };

                        request.AddBody(new { volumeId = "4YydO00I9JYC" });
                    };

                Because of = () => response = client.Execute(request);

                It should_return_http_401_unauthorized = 
                    () => response.StatusCode.ShouldEqual(HttpStatusCode.Unauthorized);

                static RestClient client;
                static RestRequest request;
                static RestResponse response;
            }

            public class when_no_book_id_is_provided
            {
                Establish context =
                    () =>
                    {
                        var apiToken = RestHelper.GenerateNewApiToken();

                        client = new RestClient("http://localhost/api/v1");
                        request = new RestRequest("previousreads")
                        {
                            Method = Method.POST,
                            RequestFormat = DataFormat.Json
                        };
                        request.AddCookie(OhbCookies.ApiToken, apiToken);
                        client.AddHandler("application/json", new DynamicJsonDeserializer());
                    };

                Because of = () => response = client.Execute<dynamic>(request);

                It should_return_http_400_bad_request = 
                    () => response.StatusCode.ShouldEqual(HttpStatusCode.BadRequest);

                static RestClient client;
                static RestRequest request;
                static RestResponse<dynamic> response;
            }

            public class when_no_matching_book_is_found
            {
                Establish context =
                    () =>
                    {
                        var apiToken = RestHelper.GenerateNewApiToken();

                        client = new RestClient("http://localhost/api/v1");
                        request = new RestRequest("previousreads")
                        {
                            Method = Method.POST,
                            RequestFormat = DataFormat.Json
                        };
                        request.AddCookie(OhbCookies.ApiToken, apiToken);

                        request.AddBody(new { volumeId = "xxxxxxxxxxxxxxx" });
                        client.AddHandler("application/json", new DynamicJsonDeserializer());
                    };

                Because of = () => response = client.Execute<dynamic>(request);

                It should_return_http_404_not_found =
                    () => response.StatusCode.ShouldEqual(HttpStatusCode.NotFound);

                static RestClient client;
                static RestRequest request;
                static RestResponse<dynamic> response;
            }

            public class when_an_http_put_is_sent
            {
                Because of =
                    () => statusCode = RestHelper.GetStatusCode("previousreads", Method.PUT);

                It should_return_http_405_method_not_allowed =
                    () => statusCode.ShouldEqual(HttpStatusCode.MethodNotAllowed);

                static HttpStatusCode statusCode;
            }

            public class when_marking_duplicate_books_as_previously_read
            {
                Establish context =
                    () =>
                    {
                        var apiToken = RestHelper.GenerateNewApiToken();

                        client = new RestClient("http://localhost/api/v1");
                        request1 = new RestRequest("previousreads")
                        {
                            Method = Method.POST,
                            RequestFormat = DataFormat.Json,
                        };
                        request1.AddCookie(OhbCookies.ApiToken, apiToken);
                        request1.AddBody(new { volumeId = "4YydO00I9JYC" });

                        request2 = new RestRequest("previousreads")
                        {
                            Method = Method.POST,
                            RequestFormat = DataFormat.Json
                        };
                        request2.AddCookie(OhbCookies.ApiToken, apiToken);

                        request2.AddBody(new { volumeId = "4YydO00I9JYC" });
                    };

                Because of = () =>
                                 {
                                     client.Execute<dynamic>(request1);
                                     response = client.Execute<dynamic>(request2);
                                 };

                It should_return_http_200_ok = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);

                static RestResponse<dynamic> response;
                static RestClient client;
                static RestRequest request1;
                static RestRequest request2;
            }
        }
    }
}
