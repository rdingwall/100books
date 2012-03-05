using System;
using System.Web;
using Machine.Specifications;
using Ohb.Mvc.AuthCookies;
using Ohb.Mvc.Storage.Users;
using Rhino.Mocks;

namespace Ohb.Mvc.Specs.AuthCookies
{
    public class AuthCookieFactorySpecs
    {
        [Subject(typeof(AuthCookieFactory))]
        public class when_creating_an_auth_cookie
        {
            Establish context =
                () =>
                    {
                        expected = "test cookie";
                        user = new User {Id = "test user id"};
                        encoder = MockRepository.GenerateStub<IAuthCookieEncoder>();
                        encoder.Stub(e => e.Encode(Arg<AuthCookieContext>.Matches(c => c.UserId.Equals(user.Id))))
                            .Return(expected);
                        factory = new AuthCookieFactory(encoder);
                    };

            Because of = () => cookie = factory.CreateAuthCookie(user);

            It should_have_the_correct_name = () => cookie.Name.ShouldEqual(OhbCookies.AuthCookie);

            It should_expire_after_the_correct_period =
                () => cookie.Expires.ShouldBeCloseTo(DateTime.UtcNow.Add(AuthCookieFactory.ExpiryDuration),
                                                     TimeSpan.FromSeconds(5));

            It should_have_the_encoded_auth_cookie_value = () => cookie.Value.ShouldEqual(expected);

            static User user;
            static IAuthCookieFactory factory;
            static IAuthCookieEncoder encoder;
            static HttpCookie cookie;
            static string expected;
        }
    }
}