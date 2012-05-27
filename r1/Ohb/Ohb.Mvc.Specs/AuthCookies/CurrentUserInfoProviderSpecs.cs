using System.Collections.Generic;
using System.Web;
using Machine.Specifications;
using Ohb.Mvc.AuthCookies;
using Rhino.Mocks;
using System.Collections;

namespace Ohb.Mvc.Specs.AuthCookies
{
    public class CurrentUserInfoProviderSpecs
    {
        public abstract class scenario
        {
            Establish context = () =>
            {
                HttpContext = MockRepository.GenerateMock<HttpContextBase>();
                HttpContextItems = new Dictionary<string, object>();
                HttpContext.Stub(c => c.Items).Return(HttpContextItems);
                Factory = MockRepository.GenerateMock<ICurrentUserInfoFactory>();
                ExpectedUserInfo = new CurrentUserInfo();
                Provider = new CurrentUserInfoProvider(Factory)
                {
                    GetHttpContextCurrent = () => HttpContext
                };
            };

            Because of = () => UserInfo = Provider.GetCurrentUserInfo();

            protected static HttpContextBase HttpContext;
            protected static CurrentUserInfo ExpectedUserInfo;
            protected static CurrentUserInfoProvider Provider;
            protected static IDictionary HttpContextItems;
            protected static ICurrentUserInfoFactory Factory;
            protected static CurrentUserInfo UserInfo;
        }

        public class When_getting_the_current_user_info : scenario
        {
            Establish context = () => Factory.Stub(f => f.CreateFromAuthCookie(HttpContext))
                                          .Return(ExpectedUserInfo);

            It should_create_a_user_info_based_on_the_current_http_context =
                () => UserInfo.ShouldBeTheSameAs(ExpectedUserInfo);

            It should_store_the_user_context_in_the_http_context =
                () => HttpContext.AssertWasCalled(
                    c => c.Items.Add(CurrentUserInfoProvider.CacheKey, ExpectedUserInfo));
        }

        public class When_loading_for_the_second_time : scenario
        {
            Establish context = 
                () => HttpContextItems.Add(CurrentUserInfoProvider.CacheKey, ExpectedUserInfo);

            It should_return_the_existing_user_info_stored_in_the_http_context =
                () => UserInfo.ShouldBeTheSameAs(ExpectedUserInfo);
        }
    }
}