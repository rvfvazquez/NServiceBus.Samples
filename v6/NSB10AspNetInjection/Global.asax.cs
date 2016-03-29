using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using NServiceBus;
using NServiceBus.ObjectBuilder;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Lifestyle;

namespace NSB10AspNetInjection
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var container = new WindsorContainer();
            container.Register(Component.For<IControllerFactory>().Instance(new BuilderControllerFactory(container)));

            var configuration = new EndpointConfiguration(typeof(Global).Namespace);
            configuration.UseContainer<WindsorBuilder>(c => c.ExistingContainer(container));
            configuration.UsePersistence<InMemoryPersistence>();

            var busInstance = Endpoint.Start(configuration).GetAwaiter().GetResult();

            DependencyResolver.SetResolver(t =>
            {
                if(container.Kernel.HasComponent(t))
                {
                    return container.Resolve(t);
                }

                //default value expected by MVC to signal that we have no components of type "t"
                return null;
            },
            t =>
            {
                if(container.Kernel.HasComponent(t))
                {
                    return container.ResolveAll(t).Cast<Object>();
                }

                //default value expected by MVC to signal that we have no components of type "t"
                return new List<Object>();
            });
        }

        public class BuilderControllerFactory : DefaultControllerFactory
        {
            readonly IWindsorContainer builder;

            public BuilderControllerFactory(IWindsorContainer builder)
            {
                this.builder = builder;
            }

            public override void ReleaseController(IController controller)
            {
                builder.Release(controller);
            }

            protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
            {
                if(controllerType == null)
                {
                    throw new HttpException(404, string.Format("The controller for path '{0}' could not be found.", requestContext.HttpContext.Request.Path));
                }

                var iController = (IController)builder.Resolve(controllerType);

                return iController;
            }
        }

        public class BuilderDependencyResolver : System.Web.Http.Dependencies.IDependencyResolver
        {
            readonly IWindsorContainer builder;

            public BuilderDependencyResolver(IWindsorContainer builder)
            {
                this.builder = builder;
            }

            public System.Web.Http.Dependencies.IDependencyScope BeginScope()
            {
                var scope = this.builder.BeginScope();
                return new BuilderDependencyScope(this.builder, scope);
            }

            public object GetService(Type serviceType)
            {
                if(this.builder.Kernel.HasComponent(serviceType))
                {
                    return this.builder.Resolve(serviceType);
                }

                return null;
            }

            public IEnumerable<object> GetServices(Type serviceType)
            {
                if(this.builder.Kernel.HasComponent(serviceType))
                {
                    return this.builder.ResolveAll(serviceType).Cast<Object>();
                }

                return new Object[ 0 ];
            }

            public void Dispose()
            {

            }
        }

        class BuilderDependencyScope : System.Web.Http.Dependencies.IDependencyScope
        {
            readonly IWindsorContainer builder;
            IDisposable scope;

            public BuilderDependencyScope(IWindsorContainer scopedBuilder, IDisposable scope)
            {
                this.builder = scopedBuilder;
                this.scope = scope;
            }
            public object GetService(Type serviceType)
            {
                if(this.builder.Kernel.HasComponent(serviceType))
                {
                    return this.builder.Resolve(serviceType);
                }

                return null;
            }

            public IEnumerable<object> GetServices(Type serviceType)
            {
                if(this.builder.Kernel.HasComponent(serviceType))
                {
                    return this.builder.ResolveAll(serviceType).Cast<Object>();
                }

                return new Object[ 0 ];
            }

            public void Dispose()
            {
                this.scope.Dispose();
                this.scope = null;
                GC.SuppressFinalize(this);
            }
        }
    }
}