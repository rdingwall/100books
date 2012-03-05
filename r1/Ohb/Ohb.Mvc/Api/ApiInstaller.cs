using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Ohb.Mvc.Api.ActionFilters;
using Ohb.Mvc.Api.Controllers;

namespace Ohb.Mvc.Api
{
    public class ApiInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<RavenDbApiAttribute>(),
                Component.For<RequiresAuthCookieApiAttribute>());
            
            container.Register(
                Component.For<BooksController>().LifeStyle.Transient,
                Component.For<PreviousReadsController>().LifeStyle.Transient);
        }
    }
}