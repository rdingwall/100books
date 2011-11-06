using Bootstrap.Windsor;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Ohb.Mvc.Controllers;

namespace Ohb.Mvc.Startup
{
    public class WindsorRegistration : IWindsorRegistration
    {
        public void Register(IWindsorContainer container)
        {
            container.Register(
                Component.For<HomeController>(),
                Component.For<AccountController>());
        }
    }
}