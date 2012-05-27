using System;
using System.Net;
using System.Web;
using System.Web.Http;
using Machine.Specifications;
using Ohb.Mvc.AuthCookies;
using Rhino.Mocks;

namespace Ohb.Mvc.Specs.AuthCookies
{
    [Subject(typeof(CurrentUserInfoFactory))]
    public class OhbUserContextFactorySpecs
    {
        public class when_there_is_no_auth_cookie_present
        {
            Establish context =
                () =>
                {
                    var encoder = MockRepository.GenerateMock<IAuthCookieEncoder>();
                    factory = new CurrentUserInfoFactory(encoder);
                    httpContext = MockRepository.GenerateStub<HttpContextBase>();
                    httpContext.Stub(c => c.Request.Cookies).Return(new HttpCookieCollection());
                };

            Cleanup after = () => factory.Dispose();

            Because of = () => userInfo = factory.CreateFromAuthCookie(httpContext);

            It should_not_be_logged_in = () => userInfo.IsAuthenticated.ShouldBeFalse();

            static CurrentUserInfo userInfo;
            static ICurrentUserInfoFactory factory;
            static HttpContextBase httpContext;
        }

        public class when_there_is_an_auth_cookie
        {
            Establish context =
                () =>
                {
                    const string cookieBase64 = "test cookie";
                    var encoder = MockRepository.GenerateMock<IAuthCookieEncoder>();
                    AuthCookieContext dummy;
                    userId = "test user id";
                    encoder
                        .Stub(e => e.TryDecode(cookieBase64, out dummy)).Return(true)
                        .OutRef(new AuthCookieContext { UserId = userId});

                    factory = new CurrentUserInfoFactory(encoder);
                    httpContext = MockRepository.GenerateStub<HttpContextBase>();

                    httpContext.Stub(c => c.Request.Cookies).Return(
                        new HttpCookieCollection
                            {
                                    new HttpCookie(OhbCookies.AuthCookie, cookieBase64)
                                });
                };

            Cleanup after = () => factory.Dispose();

            Because of = () => userInfo = factory.CreateFromAuthCookie(httpContext);

            It should_be_logged_in = () => userInfo.IsAuthenticated.ShouldBeTrue();
            It should_return_the_user_id = () => userInfo.UserId.ShouldEqual(userId);

            static CurrentUserInfo userInfo;
            static ICurrentUserInfoFactory factory;
            static HttpContextBase httpContext;
            static string userId;
        }

        public class when_there_is_a_bad_auth_cookie
        {
            Establish context =
                () =>
                {
                    const string cookieBase64 = "test cookie";
                    var encoder = MockRepository.GenerateMock<IAuthCookieEncoder>();
                    AuthCookieContext dummy;
                    encoder
                        .Stub(e => e.TryDecode(cookieBase64, out dummy)).Return(false);

                    factory = new CurrentUserInfoFactory(encoder);
                    httpContext = MockRepository.GenerateStub<HttpContextBase>();

                    httpContext.Stub(c => c.Request.Cookies).Return(
                        new HttpCookieCollection
                            {
                                    new HttpCookie(OhbCookies.AuthCookie, cookieBase64)
                                });
                };

            Cleanup after = () => factory.Dispose();

            Because of = () => exception = Catch.Exception(() => factory.CreateFromAuthCookie(httpContext));

            It should_throw_an_http_exception = 
                () => exception.ShouldBe(typeof(HttpResponseException));

            It should_have_a_401_unauthorized_status_code =
                () => ((HttpResponseException) exception).Response.StatusCode.ShouldEqual(HttpStatusCode.Unauthorized);

            static CurrentUserInfo userInfo;
            static ICurrentUserInfoFactory factory;
            static HttpContextBase httpContext;
            static string userId;
            static Exception exception;
        }
    }
}