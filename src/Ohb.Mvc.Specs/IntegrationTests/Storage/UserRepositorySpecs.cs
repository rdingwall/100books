using System;
using Machine.Specifications;
using System.Linq;
using Ohb.Mvc.Storage.Users;
using Raven.Abstractions.Exceptions;

namespace Ohb.Mvc.Specs.IntegrationTests.Storage
{
    [Subject(typeof(UserRepository))]
    public class UserRepositorySpecs
    {
        public class when_storing_a_user
        {
            Establish context =
                () =>
                    {
                        original = new User
                                       {
                                           FacebookId = "Test facebook ID",
                                           DisplayName = "Test user",
                                           ProfileImageUrl = "http://foo/bar"
                                       };
                        RavenDb.SpinUpNewDatabase();
                        repository = new UserRepository();
                    };

            Because of = () =>
                             {
                                 using (var session = RavenDb.OpenSession())
                                 {
                                     repository.AddUser(original, session);
                                     returnedUser = repository.GetFacebookUser("Test facebook ID", session);
                                 }
                             };

            It should_store_the_user = () => returnedUser.ShouldNotBeNull();

            It should_store_the_facebook_id = 
                () => returnedUser.FacebookId.ShouldEqual(original.FacebookId);

            It should_store_the_name =
                () => returnedUser.DisplayName.ShouldEqual(original.DisplayName);

            It should_retrieve_the_ravendb_id =
                () => returnedUser.Id.ShouldNotBeEmpty();

            static IUserRepository repository;
            static User original;
            static User returnedUser;
        }

        public class when_attempting_to_add_duplicates
        {
            Establish context =
                () =>
                {
                    RavenDb.SpinUpNewDatabase();
                    repository = new UserRepository();

                    using (var session = RavenDb.OpenSession())
                    {
                        repository.AddUser(new User
                        {
                            FacebookId = "123",
                            DisplayName = "First",
                            ProfileImageUrl = "http://foo/bar"
                        }, session);
                    }
                };

            Because of = () => exception = Catch.Exception(
                () =>
                    {
                        using (var session = RavenDb.OpenSession())
                        {
                            repository.AddUser(new User
                                                   {
                                                       FacebookId = "123",
                                                       DisplayName = "Second",
                                                       ProfileImageUrl = "http://foo/bar"
                                                   }, session);
                        }
                    });

            It should_throw_an_exception =
                () => exception.ShouldBe(typeof(ConcurrencyException));

            It should_not_store_the_duplicate =
                () =>
                    {
                        using (var session = RavenDb.OpenSession())
                        {
                            session.Query<User>()
                                .Customize(a => a.WaitForNonStaleResults())
                                .Where(u => u.FacebookId == "123")
                                .Single()
                                .DisplayName.ShouldEqual("First");
                        }
                    };

            static IUserRepository repository;
            static Exception exception;
        }
    }
}