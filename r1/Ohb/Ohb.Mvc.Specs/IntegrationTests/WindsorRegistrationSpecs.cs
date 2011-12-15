using System;
using System.Collections.Generic;
using System.Linq;
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
        public class when_resolving_controllers
        {
            Establish context =
                () =>
                    {
                        container = new WindsorContainer();
                        
                        // Fake one (doesn't depend on HttpContext.Current)
                        container.Register(Component.For<IUserContext>().Instance(
                            MockRepository.GenerateStub<IUserContext>()));

                        new WindsorRegistration().Register(container);

                        controllers = typeof (BooksApiController).Assembly.GetTypes()
                            .Where(t => typeof (Controller).IsAssignableFrom(t))
                            .Except(new[] {typeof(ElmahController)});
                    };

            Cleanup after = () => container.Dispose();

            It should_be_able_to_resolve_all_controllers =
                () =>
                    {
                        foreach (var controller in controllers)
                            container.Resolve(controller).ShouldNotBeNull();
                    };

            static WindsorContainer container;
            static IEnumerable<Type> controllers;
        }
    }
}