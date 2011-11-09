using Bootstrap.Windsor;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Ohb.Mvc.Amazon;
using Ohb.Mvc.Controllers;
using Ohb.Mvc.Services;
using Property = Castle.MicroKernel.Registration.Property;

namespace Ohb.Mvc.Startup
{
    public class WindsorRegistration : IWindsorRegistration
    {
        public void Register(IWindsorContainer container)
        {
            container.Register(
                Component.For<HomeController>().LifeStyle.Transient,
                Component.For<AccountController>().LifeStyle.Transient,
                Component.For<ProfileController>().LifeStyle.Transient,
                Component.For<SearchController>().LifeStyle.Transient);

            container.Register(Component.For<IUserFactory>().ImplementedBy<UserFactory>(),
                               Component.For<IUserRepository>().ImplementedBy<UserRepository>(),
                               Component.For<IUserContextFactory>().ImplementedBy<UserContextFactory>(),
                               Component.For<IAmazonBookSearchService>().ImplementedBy<AmazonBookSearchService>()
                                   .DependsOn(new
                                   {
                                       accessKeyId = "AKIAJ3XQI6KPX6JBP7SA",
                                       secretKey = "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"
                                   }));

            container.Register(
                Component.For<IUserContext>()
                    .UsingFactoryMethod(k => k.Resolve<IUserContextFactory>().GetCurrentContext())
                    .LifeStyle.PerWebRequest);
        }
    }
}