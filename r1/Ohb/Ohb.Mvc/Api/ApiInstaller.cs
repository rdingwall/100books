using System.Web.Http.Filters;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Ohb.Mvc.Api.ActionFilters;
using Ohb.Mvc.Api.Controllers;
using Ohb.Mvc.Api.Models;

namespace Ohb.Mvc.Api
{
    public class ApiInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<GoogleAnalyticsTrackerApiAttribute>()
                    .DependsOn(new {trackingId = "UA-29788114-1"}),
                Component.For<IApiModelMapper>().ImplementedBy<ApiModelMapper>(),
                AllTypes.FromThisAssembly().BasedOn<FilterAttribute>());

            container.Register(
                AllTypes.FromThisAssembly().BasedOn<OhbApiController>()
                    .Configure(c => c.LifeStyle.Transient));
        }
    }
}