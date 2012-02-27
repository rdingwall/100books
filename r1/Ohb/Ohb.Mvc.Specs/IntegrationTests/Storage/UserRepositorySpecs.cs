using Machine.Specifications;
using Ohb.Mvc.Services;

namespace Ohb.Mvc.Specs.IntegrationTests.Storage
{
#if false
    [Subject(typeof(UserRepository))]
    public class UserRepositorySpecs
    {
        public class when_storing_a_user
        {
            Establish context =
                () =>
                    {
                        original = new User(123, "Test user", "http://foo/bar");
                        TestRavenDb.UseNewTenant();
                        repository = new UserRepository();
                    };

            Because of = () =>
                             {
                                 using (var session = TestRavenDb.OpenSession())
                                 {
                                     repository.AddUser(original, session);
                                     returnedUser = repository.GetUser(123, session);
                                 }
                             };

            It should_store_the_user = () => returnedUser.ShouldNotBeNull();

            It should_store_the_facebook_id = 
                () => returnedUser.FacebookId.ShouldEqual(original.FacebookId);

            It should_store_the_name =
                () => returnedUser.Name.ShouldEqual(original.Name);

            It should_store_the_id =
                () => returnedUser.FacebookId.ShouldEqual(original.FacebookId);

            static IUserRepository repository;
            static User original;
            static IUser returnedUser;
        }

    }
#endif
}