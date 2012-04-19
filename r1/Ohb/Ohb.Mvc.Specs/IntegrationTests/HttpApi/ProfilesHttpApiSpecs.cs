using System.Linq;
using System.Net;
using Machine.Specifications;
using Ohb.Mvc.Api.Models;
using RestSharp;

namespace Ohb.Mvc.Specs.IntegrationTests.HttpApi
{
    [Subject("api/v1/profiles/:id GET"), Tags("Integration")]
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
                () => response.Data.DisplayName.ShouldStartWith(api.User.DisplayName);

            It should_get_the_users_profile_image_url =
                () => response.Data.ProfileImageUrl.ShouldEqual(api.User.ProfileImageUrl);

            It should_get_the_books_publishers =
                () => response.Data.RecentReads.Single().Id.ShouldEqual("4YydO00I9JYC");

            static RestResponse<ProfileModel> response;
            static ApiClient api;
        }

        public class when_looking_up_the_current_users_id
        {
            Establish context = () =>
            {
                api = ApiClientFactory.NewUser();
                api.MarkBookAsRead("4YydO00I9JYC");
            };

            Because of = () => response = api.GetMyProfile();

            It should_return_http_200_ok =
                () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);

            It should_be_json =
                () => response.ContentType.ShouldEqual("application/json; charset=utf-8");

            It should_get_the_users_id =
                () => response.Data.Id.ShouldEqual(api.UserId);

            It should_get_the_users_name =
                () => response.Data.DisplayName.ShouldStartWith(api.User.DisplayName);

            It should_get_the_users_profile_image_url =
                () => response.Data.ProfileImageUrl.ShouldEqual(api.User.ProfileImageUrl);

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

        public class when_requesting_the_current_users_id_but_no_auth_cookie_provided
        {
            It should_return_http_401_unauthorized =
                () => new ApiClient().AssertReturns(Method.GET, "profiles/me", HttpStatusCode.Unauthorized);
        }

        public class when_no_matching_user_is_found
        {
            It should_return_http_404_not_found =
                () => new ApiClient().AssertReturns(Method.GET, "profiles/xxxxxxxxxxxxx", HttpStatusCode.NotFound);
        }

        public class when_sending_the_wrong_http_method
        {
            Establish context = () => userId = ApiClientFactory.NewUser().UserId;

            It post_should_return_http_405_method_not_allowed =
                () => new ApiClient().AssertMethodNotAllowed(Method.POST, "profiles/" + userId);

            It put_should_return_http_405_method_not_allowed =
                () => new ApiClient().AssertMethodNotAllowed(Method.PUT, "profiles/" + userId);
        
            It delete_should_return_http_405_method_not_allowed =
                () => new ApiClient().AssertMethodNotAllowed(Method.DELETE, "profiles/" + userId);

            static string userId;
        }
    }
}
