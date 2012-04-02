using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Ohb.Mvc.Startup
{
    public class WebInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                AllTypes.FromThisAssembly().BasedOn<Controller>()
                .Configure(c => c.LifeStyle.Transient));

            container.Register(
                AllTypes.FromThisAssembly().BasedOn<FilterAttribute>());
        }
    }
}