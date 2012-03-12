using System.Linq;
using System.Net;
using Machine.Specifications;
using Ohb.Mvc.Api.Models;
using RestSharp;

namespace Ohb.Mvc.Specs.IntegrationTests.Http
{
    [Subject("api/v1/profiles/:id GET")]
    class ProfilesHttpApiSpecs
    {
        public class when_looking_up_a_profile_by_id
        {
            Establish context = () =>
                                    {
                                        api = ApiClientFactory.NewUser();
                                        api.MarkBookAsRead("4YydO00I9JYC");
                                    };

            Because of = () => response = api.GetProfile(api.UserId);

            It should_return_http_200_ok =
                () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);

            It should_be_json =
                () => response.ContentType.ShouldEqual("application/json; charset=utf-8");

            It should_get_the_users_id =
                () => response.Data.Id.ShouldEqual(api.UserId);

            It should_get_the_users_name =
                () => response.Data.Name.ShouldStartWith(api.User.Name);

            It should_get_the_users_image_url =
                () => response.Data.ImageUrl.ShouldEqual(api.User.ProfilePictureUrl);

            It should_get_the_books_publishers =
                () => response.Data.RecentReads.Single().Id.ShouldEqual("4YydO00I9JYC");

            static RestResponse<ProfileModel> response;
            static ApiClient api;
        }

        public class when_no_book_id_is_provided
        {
            It should_return_http_400_bad_request =
                () => new ApiClient().AssertReturns(Method.GET, "profiles/", HttpStatusCode.BadRequest);
        }

        public class when_no_matching_user_is_found
        {
            It should_return_http_404_not_found =
                () => new ApiClient().AssertReturns(Method.GET, "profiles/xxxxxxxxxxxxx", HttpStatusCode.NotFound);
        }

        public class when_an_http_post_is_sent
        {
            It should_return_http_405_method_not_allowed =
                () => new ApiClient().AssertMethodNotAllowed(Method.POST, "profiles/me");
        }

        public class when_an_http_put_is_sent
        {
            It should_return_http_405_method_not_allowed =
                () => new ApiClient().AssertMethodNotAllowed(Method.PUT, "profiles/me");
        }

        public class when_an_http_delete_is_sent
        {
            It should_return_http_405_method_not_allowed =
                () => new ApiClient().AssertMethodNotAllowed(Method.DELETE, "profiles/me");
        }
    }
}
