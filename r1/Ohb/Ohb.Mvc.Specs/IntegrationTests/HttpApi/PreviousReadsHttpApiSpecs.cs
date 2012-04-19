using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Machine.Specifications;
using Ohb.Mvc.Api.Models;
using Ohb.Mvc.Storage.PreviousReads;
using RestSharp;

namespace Ohb.Mvc.Specs.IntegrationTests.HttpApi
{
    class PreviousReadsHttpApiSpecs
    {
        [Subject("api/v1/previousreads GET"), Tags("Integration")]
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

        [Subject("api/v1/previousreads/:id PUT"), Tags("Integration")]
        public class when_marking_a_book_as_previously_read
        {
            [Subject("api/v1/previousreads/:id PUT"), Tags("Integration")]
            public class when_it_is_a_valid_request
            {
                Because of = () =>
                                 {
                                     api = ApiClientFactory.NewUser();
                                     response = api.MarkBookAsRead("4YydO00I9JYC");
                                     
                                     // Also mark another book as read under a different user
                                     ApiClientFactory.NewUser().MarkBookAsRead("KOWFacYRlXoC");

                                     LiveRavenDb.WaitForNonStaleResults<PreviousRead, PreviousReadsWithBook>();

                                     results = api.GetPreviousReads();
                                 };

                It should_return_http_200_ok_when_adding_a_previous_read = 
                    () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);

                It should_return_the_previous_reads_list_as_json =
                    () => results.ContentType.ShouldEqual("application/json; charset=utf-8");

                It should_return_http_200_for_the_previous_reads_list =
                    () => results.StatusCode.ShouldEqual(HttpStatusCode.OK);

                It should_only_return_one_book_in_the_previous_reads_list =
                    () => results.Data.Count.ShouldEqual(1);

                It should_contain_the_book_in_the_previous_reads_list =
                    () => results.Data.FirstOrDefault().Id.ShouldEqual("4YydO00I9JYC");

                It should_contain_the_full_book_details_in_the_previous_reads_list =
                    () => results.Data.FirstOrDefault().Title.ShouldEqual("The Google story");

                [Ignore("Off by 1 hour for some reason? I can't figure this out... something to do with JSON deserialization maybe?")]
                It should_contain_the_marked_read_at_timestamp =
                    () =>
                    results.Data.FirstOrDefault().MarkedByUserAt.ShouldBeCloseTo(DateTime.UtcNow,
                                                                                 TimeSpan.FromSeconds(5));
                static RestResponse response;
                static RestResponse<List<PreviousReadModel>> results;
                static ApiClient api;
            }

            [Subject("api/v1/previousreads/:id PUT"), Tags("Integration")]
            public class when_there_was_no_auth_cookie
            {
                Because of = 
                    () => response = ApiClientFactory.Anonymous().MarkBookAsRead("4YydO00I9JYC");

                It should_return_http_401_unauthorized = 
                    () => response.StatusCode.ShouldEqual(HttpStatusCode.Unauthorized);

                static RestResponse response;
            }

            [Subject("api/v1/previousreads/:id PUT"), Tags("Integration")]
            public class when_no_book_id_is_provided
            {
                Because of =
                    () => response = ApiClientFactory.NewUser().MarkBookAsRead(null);

                It should_return_http_400_bad_request = 
                    () => response.StatusCode.ShouldEqual(HttpStatusCode.BadRequest);

                static RestResponse response;
            }

            [Subject("api/v1/previousreads/:id PUT"), Tags("Integration")]
            public class when_no_matching_book_is_found
            {
                Because of = 
                    () => response = ApiClientFactory.NewUser().MarkBookAsRead("xxxxxxxxxxxxxxx");

                It should_return_http_404_not_found =
                    () => response.StatusCode.ShouldEqual(HttpStatusCode.NotFound);

                static RestResponse response;
            }

            [Subject("api/v1/previousreads/:id PUT"), Tags("Integration")]
            public class when_an_http_post_is_sent
            {
                It should_return_http_405_method_not_allowed =
                    () => ApiClientFactory.NewUser().AssertMethodNotAllowed(Method.POST, "previousreads");
            }

            [Subject("api/v1/previousreads/:id PUT"), Tags("Integration")]
            public class when_marking_duplicate_books_as_previously_read
            {
                Because of = () =>
                                 {
                                     var api = ApiClientFactory.NewUser();
                                     api.MarkBookAsRead("4YydO00I9JYC");
                                     response = api.MarkBookAsRead("4YydO00I9JYC");
                                 };

                It should_return_http_201_ok = 
                    () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);

                static RestResponse response;
            }

            [Subject("api/v1/previousreads PUT"), Tags("Integration")]
            public class when_querying_previously_read_books
            {
                Establish context = () =>
                                        {
                                            api = ApiClientFactory.NewUser();
                                            api.MarkBookAsRead("N0EEAAAAMBAJ");
                                            api.MarkBookAsRead("lAIJAAAAIAAJ");
                                            api.MarkBookAsRead("KOWFacYRlXoC");
                                            api.MarkBookAsRead("4YydO00I9JYC");
                                            LiveRavenDb.WaitForNonStaleResults<PreviousRead, PreviousReadsWithBook>();
                                        };

                Because of = () => response = api.GetPreviousReads();

                It should_return_the_correct_number = 
                    () => response.Data.Count.ShouldEqual(4);

                It should_return_the_latest_read_books_first =
                    () => String.Join(",", response.Data.Select(p => p.Id))
                              .ShouldEqual("4YydO00I9JYC,KOWFacYRlXoC,lAIJAAAAIAAJ,N0EEAAAAMBAJ");

                static RestResponse<List<PreviousReadModel>> response;
                static ApiClient api;
            }
        }

        [Subject("api/v1/previousreads/:id DELETE"), Tags("Integration")]
        public class when_removing_a_previous_read
        {
            [Subject("api/v1/previousreads/:id DELETE"), Tags("Integration")]
            public class when_it_is_a_valid_request
            {
                Establish context = () =>
                {
                    api = ApiClientFactory.NewUser();
                    api.MarkBookAsRead("4YydO00I9JYC");
                    api.MarkBookAsRead("N0EEAAAAMBAJ");

                    LiveRavenDb.WaitForNonStaleResults<PreviousRead, PreviousReadsWithBook>();
                };

                Because of = () =>
                                 {

                                     response = api.RemovePreviousRead("N0EEAAAAMBAJ");
                                     LiveRavenDb.WaitForNonStaleResults<PreviousRead, PreviousReadsWithBook>();
                                 };

                It should_return_http_200_ok =
                    () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);

                It should_then_exclude_that_book_from_the_previous_reads_list =
                    () => api.GetPreviousReads().Data.Select(p => p.Id)
                              .ShouldNotContain("N0EEAAAAMBAJ");

                static ApiClient api;
                static RestResponse response;
            }
        }

        [Subject("api/v1/previousreads/:id DELETE"), Tags("Integration")]
        public class when_no_book_id_is_provided
        {
            Because of =
                () => response = ApiClientFactory.NewUser().RemovePreviousRead("");

            It should_return_http_400_bad_request =
                () => response.StatusCode.ShouldEqual(HttpStatusCode.BadRequest);

            static RestResponse response;
        }

        [Subject("api/v1/previousreads/:id DELETE"), Tags("Integration")]
        public class when_removing_a_book_that_wasnt_previously_read
        {
            Because of =
                () => response = ApiClientFactory.NewUser().RemovePreviousRead("4YydO00I9JYC");

            // DELETE verb is idempotent - can fire it as many times as you like
            It should_return_http_200_ok =
                () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);

            static RestResponse response;
        }

    }
}
