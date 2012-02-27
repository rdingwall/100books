using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Castle.Windsor;
using Microsoft.Practices.ServiceLocation;

namespace Ohb.Mvc.Startup
{
    // Can't use the default Windsor service locator adapter because MVC3
    // expects some slightly weird behaviour from it. Using this one from
    // http://stackoverflow.com/questions/4140860/castle-windsor-dependency-resolver-for-mvc-3
    public class WindsorResolver : IDependencyResolver, IServiceLocator
    {
        private readonly IWindsorContainer container;

        public WindsorResolver(IWindsorContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");
            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            return container.Kernel.HasComponent(serviceType) ? container.Resolve(serviceType) : null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return container.Kernel.HasComponent(serviceType) ? container.ResolveAll(serviceType).Cast<object>() : new object[] { };
        }

        public object GetInstance(Type serviceType)
        {
            return container.Kernel.HasComponent(serviceType) ? container.Resolve(serviceType) : null;
        }

        public object GetInstance(Type serviceType, string key)
        {
            return container.Resolve(key, serviceType);
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return (IEnumerable<object>) container.ResolveAll(serviceType);
        }

        public TService GetInstance<TService>()
        {
            return container.Resolve<TService>();
        }

        public TService GetInstance<TService>(string key)
        {
            return container.Resolve<TService>(key);
        }

        public IEnumerable<TService> GetAllInstances<TService>()
        {
            return container.ResolveAll<TService>();
        }
    }
}