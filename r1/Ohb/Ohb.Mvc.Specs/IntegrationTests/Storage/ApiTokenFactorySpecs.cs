using Machine.Specifications;
using Ohb.Mvc.Storage;
using Ohb.Mvc.Storage.ApiTokens;
using Rhino.Mocks;
using System.Linq;

namespace Ohb.Mvc.Specs.IntegrationTests.Storage
{
    [Subject(typeof(ApiTokenFactory))]
    public class ApiTokenFactorySpecs
    {
        public abstract class scenario
        {
            Establish context =
                () =>
                    {
                        generator = MockRepository.GenerateStub<ICryptoTokenGenerator>();
                        provider = new ApiTokenFactory(generator, new RavenUniqueInserter());
                    };

            protected static ICryptoTokenGenerator generator;
            protected static IApiTokenFactory provider;
        }

        public class When_an_unused_secret_key_was_found : scenario
        {
            Establish context = () =>
                                    {
                                        RavenDb.SpinUpNewDatabase();

                                        // use a real generator
                                        provider = new ApiTokenFactory(new CryptoTokenGenerator(), 
                                            new RavenUniqueInserter());
                                    };

            Because of = () =>
                             {
                                 using (var session = RavenDb.OpenSession())
                                     apiToken = provider.CreateApiToken("1234", session);
                             };

            It should_return_a_key = () => apiToken.ShouldNotBeNull();

            It should_have_the_users_id = () => apiToken.UserId.ShouldEqual("1234");

            It should_have_a_token = () => apiToken.Token.ShouldNotBeEmpty();

            It should_store_the_key_in_ravendb =
                () =>
                    {
                        using (var session = RavenDb.OpenSession())
                        {
                            var doc = session.Query<ApiToken>()
                                .Customize(a => a.WaitForNonStaleResults())
                                .FirstOrDefault(k => k.UserId == "1234");

                            doc.ShouldNotBeNull();
                        }
                    };

            static ApiToken apiToken;
        }

        public class When_existing_keys_were_generated : scenario
        {
            Establish context =
                () =>
                    {
                        RavenDb.SpinUpNewDatabase();

                        generator.Stub(g => g.GetNext()).Return("aaa").Repeat.Once();
                        generator.Stub(g => g.GetNext()).Return("bbb").Repeat.Once();
                        generator.Stub(g => g.GetNext()).Return("ccc");

                        using (var session = RavenDb.OpenSession())
                        {
                            var wrapper = new RavenUniqueInserter();

                            var aaa = new ApiToken {Token = "aaa"};
                            var bbb = new ApiToken {Token = "bbb"};

                            wrapper.StoreUnique(session, aaa, t => t.Token);
                            wrapper.StoreUnique(session, bbb, t => t.Token);
                            session.SaveChanges();

                            // wait
                            session.Query<ApiToken>()
                                .Customize(a => a.WaitForNonStaleResults())
                                .Any();
                        }
                    };

            Because of = () =>
            {
                using (var session = RavenDb.OpenSession())
                    apiToken = provider.CreateApiToken("1234", session);
            };

            It should_return_the_first_unused_key = () => apiToken.Token.ShouldEqual("ccc");

            It should_store_the_key_in_ravendb =
                () =>
                {
                    using (var session = RavenDb.OpenSession())
                    {
                        var doc = session.Query<ApiToken>()
                            .Customize(a => a.WaitForNonStaleResults())
                            .FirstOrDefault(k => k.Token == "ccc");

                        doc.ShouldNotBeNull();
                    }
                };

            static ApiToken apiToken;
        }
    }
}