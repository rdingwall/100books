using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Machine.Specifications;
using Ohb.Mvc.Api.ActionFilters;
using Ohb.Mvc.Api.Controllers;
using Ohb.Mvc.Storage.Users;
using Rhino.Mocks;

namespace Ohb.Mvc.Specs.Api.ActionFilters
{
#if false
    // too brittle, might revisit later
    public class ApiAuthorizeHandlerAttributeSpecs
    {
        public class DummyController : OhbApiController
        {
            void PublicAction() {}
            
            [ApiAuthorize]
            void AuthorizedAction() {}
        }

        public class ConcreteHttpActionDescriptor : HttpActionDescriptor
        {
            public string actionName;

            public override ReadOnlyCollection<HttpParameterDescriptor> GetParameters()
            {
                throw new NotImplementedException();
            }

            public override object Execute(HttpControllerContext controllerContext, IDictionary<string, object> arguments)
            {
                throw new NotImplementedException();
            }

            public override string ActionName
            {
                get { return actionName; }
            }

            public override Type ReturnType
            {
                get { throw new NotImplementedException(); }
            }
        }

        public class Before_a_public_action
        {
            Establish context =
                () =>
                    {
                        descriptor = new ConcreteHttpActionDescriptor();
                        var c = new HttpControllerContext
                                    {
                                        Controller = new DummyController(),
                                        ControllerDescriptor = new HttpControllerDescriptor()
                                    };

                        actionContext = new HttpActionContext(c, descriptor);

                        filter = new ApiAuthorizeHandlerAttribute(
                            MockRepository.GenerateStub<IUserRepository>());
                    };

            Because of =
                () =>
                    {
                        descriptor.actionName = "PublicAction";
                        exception = Catch.Exception(() => filter.OnActionExecuting(actionContext));
                    };

            It should_not_throw_any_exception = () => exception.ShouldBeNull();


            static ActionFilterAttribute filter;
            static Exception exception;
            static HttpActionContext actionContext;
            static ConcreteHttpActionDescriptor descriptor;
        }
    }
#endif
}