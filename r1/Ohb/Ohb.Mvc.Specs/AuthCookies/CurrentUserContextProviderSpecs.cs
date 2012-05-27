using System.Collections.Generic;
using System.Web;
using Machine.Specifications;
using Ohb.Mvc.AuthCookies;
using Rhino.Mocks;
using System.Collections;

namespace Ohb.Mvc.Specs.AuthCookies
{
    public class CurrentUserContextProviderSpecs
    {
        public abstract class scenario
        {
            Establish context = () =>
            {
                HttpContext = MockRepository.GenerateMock<HttpContextBase>();
                HttpContextItems = new Dictionary<string, object>();
                HttpContext.Stub(c => c.Items).Return(HttpContextItems);
                Factory = MockRepository.GenerateMock<IOhbUserContextFactory>();
                ExpectedUserContext = new OhbUserContext();
                Provider = new CurrentUserContextProvider(Factory)
                {
                    GetHttpContextCurrent = () => HttpContext
                };
            };

            Because of = () => UserContext = Provider.GetCurrentUser();

            protected static HttpContextBase HttpContext;
            protected static OhbUserContext ExpectedUserContext;
            protected static CurrentUserContextProvider Provider;
            protected static IDictionary HttpContextItems;
            protected static IOhbUserContextFactory Factory;
            protected static OhbUserContext UserContext;
        }

        public class When_getting_the_current_user : scenario
        {
            Establish context = () => Factory.Stub(f => f.CreateFromAuthCookie(HttpContext))
                                          .Return(ExpectedUserContext);

            It should_create_a_user_context_based_on_the_current_http_context =
                () => UserContext.ShouldBeTheSameAs(ExpectedUserContext);

            It should_store_the_user_context_in_the_http_context =
                () => HttpContext.AssertWasCalled(
                    c => c.Items.Add(CurrentUserContextProvider.CacheKey, ExpectedUserContext));
        }

        public class When_loading_for_the_second_time : scenario
        {
            Establish context = 
                () => HttpContextItems.Add(CurrentUserContextProvider.CacheKey, ExpectedUserContext);

            It should_return_the_existing_user_context_stored_in_the_http_context =
                () => UserContext.ShouldBeTheSameAs(ExpectedUserContext);
        }
    }
}