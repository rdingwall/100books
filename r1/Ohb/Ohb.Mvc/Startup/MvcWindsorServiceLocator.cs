using System;
using System.Collections.Generic;
using Castle.Windsor;
using Microsoft.Practices.ServiceLocation;

namespace Ohb.Mvc.Startup
{
    /// <summary>
    /// Correct behaviour for ASP.NET MVC/WebAPI is for the service locator to
    /// return null for unknown components so MVC-internal components can
    /// fall-through and be handled by the framework (whereas Windsor throws 
    /// an exception by default).
    /// </summary>
    public class MvcWindsorServiceLocator : ServiceLocatorImplBase
    {
        readonly IWindsorContainer container;

        public MvcWindsorServiceLocator(IWindsorContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");
            this.container = container;
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            if (key != null)
            {
                if (!container.Kernel.HasComponent(key))
                    return null;

                return container.Resolve(key, serviceType);
            }

            if (!container.Kernel.HasComponent(serviceType))
                return null;

            return container.Resolve(serviceType);
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return (object[]) container.ResolveAll(serviceType);
        }
    }
}