using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Ohb.Mvc.Controllers;

namespace Ohb.Mvc.Startup
{
    public class WebInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                AllTypes.FromThisAssembly().BasedOn<OhbController>()
                .Configure(c => c.LifeStyle.Transient));

            container.Register(
                AllTypes.FromThisAssembly().BasedOn<FilterAttribute>());
        }
    }
}