using System.Linq;
using System.Net;
using Machine.Specifications;
using Ohb.Mvc.Api.Models;
using Ohb.Mvc.AuthCookies;
using Ohb.Mvc.Storage.Users;
using RestSharp;

namespace Ohb.Mvc.Specs.IntegrationTests.Http
{
    public class BackdoorHttpApiSpecs
    {
        [Subject("api/backdoor/createuser POST")]
        public class when_creating_a_user
        {
            Establish context =
                () =>
                    {
                        displayName = "Test User";
                        imageUrl = "Test url";
                        api = ApiClientFactory.Anonymous();
                    };

            Because of =
                () =>
                    {
                        response = api.BackdoorCreateUser(displayName, imageUrl, setAuthCookie: true);
                        LiveRavenDb.WaitForNonStaleResults<User>();
                        profileResponse = api.GetProfile(response.Data.UserId);
                    };

            It should_return_http_200_ok =
                () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);

            It should_set_their_name =
                () => profileResponse.Data.DisplayName.ShouldEqual(displayName);

            It should_set_their_image_url =
                () => profileResponse.Data.ImageUrl.ShouldEqual(imageUrl);

            It should_set_an_auth_cookie =
                () => response.Cookies.FirstOrDefault(c => c.Name == OhbCookies.AuthCookie)
                          .Value.ShouldNotBeEmpty();

            It should_set_an_auth_cookie_for_the_requested_user =
                () =>
                {
                    var cookie = response.Cookies
                        .FirstOrDefault(c => c.Name == OhbCookies.AuthCookie)
                        .Value;

                    using (var encoder = new AuthCookieEncoder(AuthCookieSecretKey.Value))
                    {
                        AuthCookieContext ctx;
                        encoder.TryDecode(cookie, out ctx).ShouldBeTrue();
                        ctx.UserId.ShouldEqual(response.Data.UserId);
                    }
                };

            static RestResponse<BackdoorCreateUserResponse> response;
            static ApiClient api;
            static RestResponse<ProfileModel> profileResponse;
            static string displayName;
            static string imageUrl;
        }

        [Subject("api/backdoor/getauthcookie GET")]
        public class when_getting_an_auth_cookie_for_a_particular_user
        {
            Establish context = () =>
                                    {
                                        api = ApiClientFactory.NewUser();
                                        userId = api.UserId;
                                    };

            Because of = () => response = api.BackdoorGetAuthCookie(userId);

            It should_return_http_200_ok =
                () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);

            It should_set_an_auth_cookie =
                () => response.Cookies.FirstOrDefault(c => c.Name == OhbCookies.AuthCookie)
                          .Value.ShouldNotBeEmpty();

            It should_set_an_auth_cookie_for_the_requested_user =
                () =>
                    {
                        var cookie = response.Cookies
                            .FirstOrDefault(c => c.Name == OhbCookies.AuthCookie)
                            .Value;

                        using (var encoder = new AuthCookieEncoder(AuthCookieSecretKey.Value))
                        {
                            AuthCookieContext ctx;
                            encoder.TryDecode(cookie, out ctx).ShouldBeTrue();
                            ctx.UserId.ShouldEqual(userId);
                        }
                    };

            static RestResponse response;
            static string userId;
            static ApiClient api;
        }
    }
}