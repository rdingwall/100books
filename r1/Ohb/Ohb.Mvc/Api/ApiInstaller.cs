using System.Web.Http.Filters;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Ohb.Mvc.Api.Controllers;

namespace Ohb.Mvc.Api
{
    public class ApiInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                AllTypes.FromThisAssembly().BasedOn<FilterAttribute>());

            container.Register(
                AllTypes.FromThisAssembly().BasedOn<OhbApiController>()
                    .Configure(c => c.LifeStyle.Transient));
        }
    }
}