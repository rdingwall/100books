using System;
using Bootstrap.Windsor;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Ohb.Mvc.Controllers;
using Ohb.Mvc.Services;

namespace Ohb.Mvc.Startup
{
    public class WindsorRegistration : IWindsorRegistration
    {
        public void Register(IWindsorContainer container)
        {
            container.Register(
                Component.For<HomeController>().LifeStyle.Transient,
                Component.For<AccountController>().LifeStyle.Transient,
                Component.For<ProfileController>().LifeStyle.Transient);

            container.Register(Component.For<IUserFactory>().ImplementedBy<UserFactory>(),
                Component.For<IUserRepository>().ImplementedBy<UserRepository>(),
                Component.For<IUserContextFactory>().ImplementedBy<UserContextFactory>());

            container.Register(
                Component.For<IUserContext>()
                    .UsingFactoryMethod(k => k.Resolve<IUserContextFactory>().GetCurrentContext())
                    .LifeStyle.PerWebRequest);
        }
    }
}