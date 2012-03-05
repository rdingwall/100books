using System;
using Machine.Specifications;
using Ohb.Mvc.AuthCookies;

namespace Ohb.Mvc.Specs.AuthCookies
{
    public class AuthCookieEncoderSpecs
    {
        [Subject(typeof(AuthCookieEncoder))]
        public class when_encoding_an_decoding_an_auth_cookie
        {
            Establish context =
                () =>
                    {
                        // separate HMAC instances (had a bug here previously)
                        encoder1 = new AuthCookieEncoder("aaaaaa");
                        encoder2 = new AuthCookieEncoder("aaaaaa");

                        cookieContext = new AuthCookieContext
                                     {
                                         ExpirationTime = DateTime.Now.AddMilliseconds(100).AddDays(2),
                                         UserId = "users/1"
                                     };
                    };

            Cleanup after = () =>
            {
                encoder1.Dispose();
                encoder2.Dispose();
            };

            Because of = () => result = encoder2.TryDecode(encoder1.Encode(cookieContext), out decodedCookieContext);

            It should_return_true = () => result.ShouldBeTrue();

            It should_decode_the_expiry_time = 
                () => decodedCookieContext.UserId.ShouldEqual(cookieContext.UserId);

            It should_decode_the_user_id =
                () => decodedCookieContext.ExpirationTime.ShouldBeCloseTo(cookieContext.ExpirationTime, TimeSpan.FromSeconds(1));

            static IAuthCookieEncoder encoder1;
            static IAuthCookieEncoder encoder2;
            static AuthCookieContext cookieContext;
            static AuthCookieContext decodedCookieContext;
            static bool result;
        }

        [Subject(typeof(AuthCookieEncoder))]
        public class when_encoding_an_decoding_an_auth_cookie_with_an_invalid_signature
        {
            Establish context =
                () =>
                {
                    encoder1 = new AuthCookieEncoder("aaaaaa");
                    encoder2 = new AuthCookieEncoder("bbbbbb");

                    cookieContext = new AuthCookieContext
                    {
                        ExpirationTime = DateTime.Now.AddMilliseconds(100).AddDays(2),
                        UserId = "users/1"
                    };
                };

            Cleanup after = () =>
                                {
                                    encoder1.Dispose();
                                    encoder2.Dispose();
                                };

            Because of = 
                () => result = encoder2.TryDecode(encoder1.Encode(cookieContext), out decodedCookieContext);

            It should_return_false = () => result.ShouldBeFalse();

            static IAuthCookieEncoder encoder1;
            static AuthCookieContext cookieContext;
            static IAuthCookieEncoder encoder2;
            static Exception exception;
            static AuthCookieContext decodedCookieContext;
            static bool result;
        }
    }
}