using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using Ohb.Mvc.Authentication;
using Rhino.Mocks;

namespace Ohb.Mvc.Specs.Authentication
{
    public class AuthCookieCacheSpecs
    {
        [Subject(typeof(AuthCookieCache))]
        public class when_decoding_a_new_cookie
        {
            Establish context =
                () =>
                    {
                        results = new List<bool>();
                        encoder = MockRepository.GenerateMock<IAuthCookieEncoder>();
                        expected = new AuthCookieContext();
                        encoder.Stub(e => e.TryDecode(Base64, out decoded))
                            .OutRef(expected)
                            .Return(true)
                            .Repeat.Once();

                        cache = new AuthCookieCache(encoder);
                    };

            Because of = () =>
                             {
                                 results.Add(cache.TryDecode(Base64, out decoded));
                                 results.Add(cache.TryDecode(Base64, out decoded));
                                 results.Add(cache.TryDecode(Base64, out decoded));
                             };

            It should_only_invoke_the_encoder_once =
                () =>
                    {
                        AuthCookieContext dummy;
                        encoder.AssertWasCalled(
                            e => e.TryDecode(null, out dummy),
                            opt => opt.IgnoreArguments());
                    };

            It should_return_true =
                () => results.Where(r => r).Count().ShouldEqual(3);

            It should_return_the_decoded_instance =
                () => decoded.ShouldBeTheSameAs(expected);

            const string Base64 = "abc";

            static IAuthCookieEncoder encoder;
            static IAuthCookieEncoder cache;
            static AuthCookieContext decoded;
            static AuthCookieContext expected;
            static List<bool> results;
        }

        [Subject(typeof(AuthCookieCache))]
        public class when_decoding_a_cookie_that_was_previously_encoded
        {
            Establish context =
                () =>
                {
                    results = new List<bool>();
                    encoder = MockRepository.GenerateMock<IAuthCookieEncoder>();
                    original = new AuthCookieContext();

                    encoder.Stub(e => e.Encode(original)).Return(Base64).Repeat.Once();

                    cache = new AuthCookieCache(encoder);
                    cache.Encode(original);
                };

            Because of = () =>
                             {
                                 results.Add(cache.TryDecode(Base64, out decoded));
                                 results.Add(cache.TryDecode(Base64, out decoded));
                                 results.Add(cache.TryDecode(Base64, out decoded));
                             };

            It should_return_the_original =
                () => decoded.ShouldBeTheSameAs(original);

            It should_return_true =
                () => results.Where(r => r).Count().ShouldEqual(3);

            It should_never_try_to_decode_it =
                () =>
                    {
                        AuthCookieContext dummy;
                        encoder.AssertWasNotCalled(
                            e => e.TryDecode(null, out dummy),
                            opt => opt.IgnoreArguments());
                    };

            const string Base64 = "abc";
            static IAuthCookieEncoder encoder;
            static IAuthCookieEncoder cache;
            static AuthCookieContext original;
            static AuthCookieContext decoded;
            static List<bool> results;
        }
    }
}