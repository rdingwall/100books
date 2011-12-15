using Bootstrap.Windsor;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Ohb.Mvc.Amazon;
using Ohb.Mvc.Api.Controllers;
using Ohb.Mvc.Controllers;
using Ohb.Mvc.Google;
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
                Component.For<ProfileController>().LifeStyle.Transient,
                Component.For<SearchController>().LifeStyle.Transient,
                Component.For<BooksController>().LifeStyle.Transient,
                Component.For<BooksApiController>().LifeStyle.Transient);

            container.Register(Component.For<IUserFactory>().ImplementedBy<UserFactory>(),
                               Component.For<IUserRepository>().ImplementedBy<UserRepository>(),
                               Component.For<IUserContextFactory>().ImplementedBy<UserContextFactory>(),
                               Component.For<IBookSearchService>().ImplementedBy<GoogleBookSearchService>()
                                   .DependsOn(new
                                   {
                                       apiKey = "AIzaSyDQsH0G4o3l9FjHUocTO_edha6Pv8N3NXo"
                                   }),
                               Component.For<IBookSearchService>().Named("AmazonBookSearchService").ImplementedBy<AmazonBookSearchService>()
                                   .DependsOn(new
                                   {
                                       accessKeyId = "AKIAJ3XQI6KPX6JBP7SA",
                                       secretKey = "Rowkj/jkta9LOer/c6PIinMEfYe/Rt8p5SfAY/jQ"
                                   }));

            // Depends on HttpContext.Current. In tests we will inject a fake one.
            if (!container.Kernel.HasComponent(typeof(IUserContext)))
            {
                container.Register(
                    Component.For<IUserContext>()
                        .UsingFactoryMethod(k => k.Resolve<IUserContextFactory>().GetCurrentContext())
                        .LifeStyle.PerWebRequestIfPossible());
            }
        }
    }
}