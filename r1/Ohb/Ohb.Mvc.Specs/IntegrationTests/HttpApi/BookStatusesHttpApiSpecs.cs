using System.Collections.Generic;
using System.Linq;
using System.Net;
using Machine.Specifications;
using Ohb.Mvc.Api.Models;
using Ohb.Mvc.Storage.PreviousReads;
using RestSharp;

namespace Ohb.Mvc.Specs.IntegrationTests.HttpApi
{
    public class BookStatusesHttpApiSpecs
    {
        public class when_retrieving_book_statuses
        {
            [Subject("api/v1/books/:ids/statuses GET"), Tags("Integration")]
            public class when_it_is_a_valid_request
            {
                Establish context =
                    () =>
                        {
                            api = ApiClientFactory.NewUser();

                            api.MarkBookAsRead("0W0DRgAACAAJ");
                            api.MarkBookAsRead("2GZlm91NNEgC");
                            LiveRavenDb.WaitForNonStaleResults<PreviousRead>();
                        };

                Because of = () => response = api.GetBookStatuses("0W0DRgAACAAJ", "xxx",
                                                                  "2GZlm91NNEgC", "yyy", "zzz");

                It should_return_http_200_ok = 
                    () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);

                It should_be_json = 
                    () => response.ContentType.ShouldEqual("application/json; charset=utf-8");

                It should_mark_the_previously_read_books_as_read =
                    () =>
                        {
                            response.Data.Single(b => b.GoogleVolumeId == "0W0DRgAACAAJ").HasRead.ShouldBeTrue();
                            response.Data.Single(b => b.GoogleVolumeId == "2GZlm91NNEgC").HasRead.ShouldBeTrue();
                        };

                It should_mark_the_unread_books_as_unread =
                    () =>
                    {
                        response.Data.Single(b => b.GoogleVolumeId == "xxx").HasRead.ShouldBeFalse();
                        response.Data.Single(b => b.GoogleVolumeId == "yyy").HasRead.ShouldBeFalse();
                    };

                It should_return_a_status_for_each_requested_id =
                    () => response.Data.Count().ShouldEqual(5);

                static RestResponse<List<BookStatus>> response;
                static ApiClient api;
            }

            [Subject("api/v1/books/:ids/statuses POST"), Tags("Integration")]
            public class when_it_is_the_wrong_http_method
            {
                It should_return_http_405_method_not_allowed =
                    () => ApiClientFactory.NewUser()
                              .AssertMethodNotAllowed(Method.POST, "books/0W0DRgAACAAJ,2GZlm91NNEgC/statuses");
            }

            [Subject("api/v1/books/:ids/statuses GET"), Tags("Integration")]
            public class when_there_was_no_auth_token
            {
                Because of =
                    () => response = ApiClientFactory.Anonymous().GetBookStatuses("yyy", "xxx");

                It should_return_http_401_unauthorized =
                    () => response.StatusCode.ShouldEqual(HttpStatusCode.Unauthorized);

                static RestResponse<List<BookStatus>> response;
            }

            [Subject("api/v1/books/:ids/statuses GET"), Tags("Integration")]
            public class when_requesting_the_same_book_id_multiple_times
            {
                Because of =
                    () => response = ApiClientFactory.NewUser().GetBookStatuses("xxx", "xxx", "xxx");

                It should_return_http_200_ok =
                    () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);

                It should_return_the_book_only_once =
                    () => response.Data.Count.ShouldEqual(1);

                static RestResponse<List<BookStatus>> response;
            }
        }
    }
}