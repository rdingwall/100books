using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Machine.Specifications;
using Ohb.Mvc.Api.Controllers;
using Ohb.Mvc.Areas.Admin.Controllers;
using Ohb.Mvc.Services;
using Ohb.Mvc.Startup;
using Rhino.Mocks;

namespace Ohb.Mvc.Specs.IntegrationTests
{
    [Subject(typeof(WindsorRegistration))]
    public class WindsorRegistrationSpecs
    {
        public abstract class scenario
        {
            Establish context =
                () =>
                {
                    container = new WindsorContainer();

                    // Fake one (doesn't depend on HttpContext.Current)
                    container.Register(Component.For<IUserContext>().Instance(
                        MockRepository.GenerateStub<IUserContext>()));

                    new WindsorRegistration().Register(container);
                };

            Cleanup after = () => container.Dispose();

            protected static WindsorContainer container;

            protected static IEnumerable<Type> controllers;
        }

        public class when_resolving_controllers : scenario
        {
            Establish context =
                () =>
                    {
                        controllers = typeof (BooksController).Assembly.GetTypes()
                            .Where(t => typeof (Controller).IsAssignableFrom(t))
                            .Where(t => !t.IsAbstract)
                            .Except(new[] {typeof(ElmahController)});
                    };

            It should_be_able_to_resolve_all_controllers =
                () =>
                    {
                        foreach (var controller in controllers)
                            container.Resolve(controller).ShouldNotBeNull();
                    };
        }

        public class when_resolving_api_controllers : scenario
        {
            Establish context =
                () =>
                {
                    controllers = typeof(BooksController).Assembly.GetTypes()
                        .Where(t => typeof(ApiController).IsAssignableFrom(t))
                        .Where(t => !t.IsAbstract);
                };

            It should_be_able_to_resolve_all_api_controllers =
                () =>
                {
                    foreach (var controller in controllers)
                        container.Resolve(controller).ShouldNotBeNull();
                };
        }
    }
}