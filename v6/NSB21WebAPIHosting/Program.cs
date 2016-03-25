using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.Owin.Hosting;
using NServiceBus;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dependencies;

namespace NSB21WebAPIHosting
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            var container = new WindsorContainer();

            var allControllers = Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(ApiController).IsAssignableFrom(t));
            foreach(var controller in allControllers)
            {
                container.Register(Component.For(controller).LifestyleTransient());
            }


            var cfg = new EndpointConfiguration(typeof(Program).Namespace);

            cfg.UseContainer<WindsorBuilder>(c => c.ExistingContainer(container));
            cfg.UniquelyIdentifyRunningInstance()
                .UsingCustomIdentifier(new Guid("{9B589AA5-112A-4396-85EB-A222187FE0E2}"));

            cfg.EnableInstallers();

            cfg.UsePersistence<InMemoryPersistence>();

            cfg.Conventions()
                .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith(".Commands"))
                .DefiningEventsAs(t => t.Namespace != null && t.Namespace.EndsWith(".Events"));

            using(var webApiHost = StartWebAPI(new CustomDependencyResolver(container)))
            {
                var endpoint = await Endpoint.Start(cfg);

                Console.Read();

                await endpoint.Stop();
            }
        }

        static IDisposable StartWebAPI(IDependencyResolver resolver)
        {
            // Start OWIN host 
            return WebApp.Start("http://localhost:9000/", appBuilder =>
            {
                var config = new HttpConfiguration();
                config.DependencyResolver = resolver;

                config.MapHttpAttributeRoutes();

                config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );

                appBuilder.UseWebApi(config);
            });
        }
    }
}
