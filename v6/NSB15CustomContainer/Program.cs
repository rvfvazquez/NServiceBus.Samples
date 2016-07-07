using Castle.MicroKernel.Registration;
using NServiceBus;
using Radical.Bootstrapper;
using System;
using System.ComponentModel.Composition;

namespace NSB15CustomContainer
{
    class Program
    {
        static void Main(string[] args)
        {
            var bootstrapper = new WindsorBootstrapper
            (
                directory: AppDomain.CurrentDomain.BaseDirectory, //the directory where to look for assemblies
                filter: "*.*" //the default filer is *.dll, but this is and exe so we need to include it as well
            );

            //the bootstrap process will look for any class 
            //the implements the IWindsorInstaller interface
            //and is exported via MEF
            var container = bootstrapper.Boot();

            var config = new EndpointConfiguration(typeof(Program).Namespace);
            config.UsePersistence<InMemoryPersistence>();
            config.UseContainer<WindsorBuilder>(c =>
            {
                c.ExistingContainer(container);
            });
        }
    }

    [Export(typeof(IWindsorInstaller))]
    class MyInstaller : IWindsorInstaller
    {
        public void Install( Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store )
        {
            //configure the container here with custom configuration
        }
    }

}
