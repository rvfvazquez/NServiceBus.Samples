using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;

namespace NSB21WebAPIHosting
{
    class CustomDependencyResolver : IDependencyResolver
    {
        /*
         * Very trivial resolver for the purpose of the sample, a better implementation here 
         * https://github.com/RadicalFx/Radical-Boostrappers/blob/develop/src/net45/Radical.Bootstrapper.Windsor.AspNet/Infrastructure/WindsorDependencyResolver.cs
         */

        IWindsorContainer container;

        public CustomDependencyResolver(IWindsorContainer container)
        {
            this.container = container;
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }

        public object GetService(Type serviceType)
        {
            if(this.container.Kernel.HasComponent(serviceType))
            {
                return this.container.Resolve(serviceType);
            }

            return null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if(this.container.Kernel.HasComponent(serviceType))
            {
                var all = this.container.ResolveAll(serviceType);
                return all.Cast<Object>();
            }

            return new List<Object>();
        }


        public void Dispose()
        {

        }
    }
}
