using System;
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
                Component.For<BooksApiController>().LifeStyle.Transient,
                Component.For<PreviousReadsApiController>().LifeStyle.Transient);
        }
    }
}