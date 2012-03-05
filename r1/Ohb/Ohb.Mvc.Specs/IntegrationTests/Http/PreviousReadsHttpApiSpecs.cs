using System.Collections.Generic;
using System.Linq;
using System.Net;
using Machine.Specifications;
using Ohb.Mvc.Storage.Books;
using RestSharp;

namespace Ohb.Mvc.Specs.IntegrationTests.Http
{
    class PreviousReadsHttpApiSpecs
    {
        [Subject("api/v1/previousreads GET")]
        public class when_retrieving_previous_reads
        {
            public class when_there_was_no_auth_cookie
            {
                It should_return_http_401_unauthorized =
                    () => new ApiClient().AssertUnauthorized("previousreads");
            }

            public class when_there_was_an_expired_auth_cookie
            {
                // ..?
            }
        }

        [Subject("api/v1/previousreads POST")]
        public class when_marking_a_book_as_previously_read
        {
            public class when_it_is_a_valid_request
            {
                Because of = () =>
                                 {
                                     var api = ApiClientFactory.NewUser();
                                     response = api.MarkBookAsRead("4YydO00I9JYC");
                                     results = api.GetPreviousReads();
                                 };

                It should_return_http_200_ok_when_marking_the_book_as_read = 
                    () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);

                It should_return_the_previous_reads_list_as_json =
                    () => results.ContentType.ShouldEqual("application/json; charset=utf-8");

                It should_return_http_200_for_the_previous_reads_list =
                    () => results.StatusCode.ShouldEqual(HttpStatusCode.OK);

                It should_contain_the_book_in_the_previous_reads_list =
                    () => results.Data.Select(b => b.StaticInfo.Id).ShouldContain("4YydO00I9JYC");

                It should_contain_the_full_book_details_in_the_previous_reads_list =
                    () => results.Data.Select(b => b.StaticInfo.Title).ShouldContain("The Google story");

                static RestResponse response;
                static RestResponse<List<Book>> results;
            }

            public class when_there_was_no_auth_cookie
            {
                Because of = 
                    () => response = ApiClientFactory.Anonymous().MarkBookAsRead("4YydO00I9JYC");

                It should_return_http_401_unauthorized = 
                    () => response.StatusCode.ShouldEqual(HttpStatusCode.Unauthorized);

                static RestResponse response;
            }

            public class when_no_book_id_is_provided
            {
                Because of =
                    () => response = ApiClientFactory.NewUser().MarkBookAsRead(null);

                It should_return_http_400_bad_request = 
                    () => response.StatusCode.ShouldEqual(HttpStatusCode.BadRequest);

                static RestResponse response;
            }

            public class when_no_matching_book_is_found
            {
                Because of = 
                    () => response = ApiClientFactory.NewUser().MarkBookAsRead("xxxxxxxxxxxxxxx");

                It should_return_http_404_not_found =
                    () => response.StatusCode.ShouldEqual(HttpStatusCode.NotFound);

                static RestResponse response;
            }

            public class when_an_http_put_is_sent
            {
                It should_return_http_405_method_not_allowed =
                    () => ApiClientFactory.NewUser().AssertMethodNotAllowed(Method.PUT, "previousreads");
            }

            public class when_marking_duplicate_books_as_previously_read
            {
                Because of = () =>
                                 {
                                     var api = ApiClientFactory.NewUser();
                                     api.MarkBookAsRead("4YydO00I9JYC");
                                     response = api.MarkBookAsRead("4YydO00I9JYC");
                                 };

                It should_return_http_200_ok = 
                    () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);

                static RestResponse response;
            }
        }
    }
}
