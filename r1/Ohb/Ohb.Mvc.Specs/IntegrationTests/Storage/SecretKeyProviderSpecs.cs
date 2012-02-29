using System;
using Machine.Specifications;
using Ohb.Mvc.Storage;
using Rhino.Mocks;
using System.Linq;

namespace Ohb.Mvc.Specs.IntegrationTests.Storage
{
    [Subject(typeof(SecretKeyProvider))]
    public class SecretKeyProviderSpecs
    {
        public abstract class scenario
        {
            Establish context =
                () =>
                    {
                        generator = MockRepository.GenerateStub<ISecretKeyGenerator>();
                        provider = new SecretKeyProvider(generator);
                    };

            protected static ISecretKeyGenerator generator;
            protected static SecretKeyProvider provider;
        }

        public class When_an_unused_secret_key_was_found : scenario
        {
            Establish context = () =>
                                    {
                                        RavenDb.SpinUpNewDatabase();

                                        // use a real generator
                                        provider = new SecretKeyProvider(new SecretKeyGenerator());
                                    };

            Because of = () =>
                             {
                                 using (var session = RavenDb.OpenSession())
                                     key = provider.GetUniqueKey(session);
                             };

            It should_return_a_key = () => key.ShouldNotBeEmpty();

            It should_store_the_key_in_ravendb =
                () =>
                    {
                        using (var session = RavenDb.OpenSession())
                        {
                            var doc = session.Query<SecretUserKey>()
                                .Customize(a => a.WaitForNonStaleResults())
                                .FirstOrDefault(k => k.SecretKey == key);

                            doc.ShouldNotBeNull();
                        }
                    };

            static string key;
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
                            session.Store(new SecretUserKey {SecretKey = "aaa"},
                                          "SecretUserKeys/aaa");
                            session.Store(new SecretUserKey {SecretKey = "bbb"},
                                          "SecretUserKeys/bbb");
                            session.SaveChanges();

                            // wait
                            session.Query<SecretUserKey>()
                                .Customize(a => a.WaitForNonStaleResults())
                                .Any();
                        }
                    };

            Because of = () =>
            {
                using (var session = RavenDb.OpenSession())
                    key = provider.GetUniqueKey(session);
            };

            It should_return_the_first_unused_key = () => key.ShouldEqual("ccc");

            It should_store_the_key_in_ravendb =
                () =>
                {
                    using (var session = RavenDb.OpenSession())
                    {
                        var doc = session.Query<SecretUserKey>()
                            .Customize(a => a.WaitForNonStaleResults())
                            .FirstOrDefault(k => k.SecretKey == "ccc");

                        doc.ShouldNotBeNull();
                    }
                };

            static string key;
        }
    }
}