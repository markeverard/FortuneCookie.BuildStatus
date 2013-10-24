using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using StructureMap;

namespace FortuneCookie.BuildStatus.Ioc
{
    public class StructureMapDependencyResolver : IDependencyResolver
    {
        public StructureMapDependencyResolver(IContainer container)
        {
            _container = container;
        }

        private IContainer _container;

        public object GetService(Type serviceType)
        {
            object instance = _container.TryGetInstance(serviceType);

            if (instance == null && !serviceType.IsAbstract)
            {
                _container.Configure(c => c.AddType(serviceType, serviceType));
                instance = _container.TryGetInstance(serviceType);
            }

            return instance;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.GetAllInstances(serviceType).Cast<object>();
        }
    }
}