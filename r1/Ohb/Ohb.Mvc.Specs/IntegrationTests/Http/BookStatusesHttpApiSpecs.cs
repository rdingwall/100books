using System.Collections.Generic;
using System.Linq;
using System.Net;
using Machine.Specifications;
using Ohb.Mvc.Storage.PreviousReads;
using RestSharp;
using RestSharp.Deserializers;

namespace Ohb.Mvc.Specs.IntegrationTests.Http
{
    public class BookStatusesHttpApiSpecs
    {
        public class when_retrieving_book_statuses
        {
            [Subject("api/v1/books/:ids/statuses GET")]
            public class when_it_is_a_valid_request
            {
                Establish context =
                    () =>
                        {
                            var authCookie = RestHelper.GetRandomUserAuthCookie();

                            RestHelper.MarkBookAsRead("0W0DRgAACAAJ", authCookie);
                            RestHelper.MarkBookAsRead("2GZlm91NNEgC", authCookie);
                            RestHelper.WaitForNonStaleResults<PreviousRead>();
                            
                            client = new RestClient("http://localhost/api/v1");
                            client.AddHandler("application/json", new DynamicJsonDeserializer());
                            request = new RestRequest("books/0W0DRgAACAAJ,xxx,2GZlm91NNEgC,yyy,zzz/statuses");
                            request.AddCookie(OhbCookies.AuthCookie, authCookie);
                        };

                Because of = () => response = client.Execute<dynamic>(request);

                It should_return_http_200_ok = 
                    () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);

                It should_be_json = 
                    () => response.ContentType.ShouldEqual("application/json; charset=utf-8");

                It should_mark_the_previously_read_books_as_read =
                    () =>
                        {
                            var statuses = (IEnumerable<dynamic>) response.Data;
                            ((bool)statuses.Single(b => b.GoogleVolumeId == "0W0DRgAACAAJ").HasRead).ShouldBeTrue();
                            ((bool)statuses.Single(b => b.GoogleVolumeId == "2GZlm91NNEgC").HasRead).ShouldBeTrue();
                        };

                It should_mark_the_unread_books_as_unread =
                    () =>
                    {
                        var statuses = (IEnumerable<dynamic>)response.Data;
                        ((bool)statuses.Single(b => b.GoogleVolumeId == "xxx").HasRead).ShouldBeFalse();
                        ((bool)statuses.Single(b => b.GoogleVolumeId == "yyy").HasRead).ShouldBeFalse();
                    };

                It should_return_a_status_for_each_requested_id =
                    () => ((IEnumerable<dynamic>)response.Data).Count().ShouldEqual(5);

                static RestClient client;
                static RestRequest request;
                static RestResponse<dynamic> response;
            }

            [Subject("api/v1/books/:ids/statuses POST")]
            public class when_it_is_the_wrong_http_method
            {
                Because of =
                    () => statusCode = RestHelper.GetStatusCode("books/0W0DRgAACAAJ,2GZlm91NNEgC/statuses", Method.POST);

                It should_return_http_405_method_not_allowed =
                    () => statusCode.ShouldEqual(HttpStatusCode.MethodNotAllowed);

                static HttpStatusCode statusCode;
            }

            [Subject("api/v1/books/:ids/statuses GET")]
            public class when_there_was_no_auth_token
            {
                Because of =
                    () => statusCode = RestHelper.GetStatusCode("books/0W0DRgAACAAJ,2GZlm91NNEgC/statuses", Method.GET);

                It should_return_http_401_unauthorized =
                    () => statusCode.ShouldEqual(HttpStatusCode.Unauthorized);

                static HttpStatusCode statusCode;
            }
        }
    }
}