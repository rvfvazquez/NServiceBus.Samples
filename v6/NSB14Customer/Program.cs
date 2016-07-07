using Topics.Radical;
using NServiceBus;
using NServiceBus.Persistence;
using Raven.Client.Embedded;
using System;
using System.Linq;
using NSB14Customer.Messages.Events;
using Topics.Radical.Helpers;
using System.Threading;
using System.Threading.Tasks;

namespace NSB14Customer
{
    class Program
    {
        public static Boolean IsDemoMode { get; private set; }

        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            var cmdLine = CommandLine.GetCurrent();
            Program.IsDemoMode = cmdLine.Contains( "demo" );

            var cfg = new EndpointConfiguration(typeof(Program).Namespace);

            cfg.UniquelyIdentifyRunningInstance()
                .UsingNames( "Customer", Environment.MachineName );

            var embeddedSore = new EmbeddableDocumentStore
            {
                ResourceManagerId = new Guid( "{B9EC41C8-6DAB-4EF2-9805-9181F0A8B208}" ),
                DataDirectory = @"~\RavenDB\Data"
            }.Initialize();

            cfg.UsePersistence<RavenDBPersistence>()
                .DoNotSetupDatabasePermissions()
                .SetDefaultDocumentStore( embeddedSore );

            cfg.Conventions()
                .DefiningCommandsAs( t => t.Namespace != null && t.Namespace.EndsWith( ".Commands" ) )
                .DefiningEventsAs( t => t.Namespace != null && t.Namespace.EndsWith( ".Events" ) );

            var endpoint = await Endpoint.Start(cfg).ConfigureAwait(false);

            using(ConsoleColor.Red.AsForegroundColor())
            {
                Console.WriteLine("Isn't this an amazing web site? :-D");
            }

            if(Program.IsDemoMode)
            {
                await StartDemoMode(endpoint).ConfigureAwait(false);
            }
            else
            {
                Logic.Run(setup =>
                {
                    setup.DefineAction(ConsoleKey.C, "Checkout", async () =>
                    {
                        await PublishEvent(endpoint).ConfigureAwait(false);
                    });
                });
            }

            await endpoint.Stop().ConfigureAwait(false);
        }

        static async Task PublishEvent( IEndpointInstance bus ) 
        {
            using( ConsoleColor.Cyan.AsForegroundColor() )
            {
                Console.WriteLine( "Publishing IShoppingCartCheckedout..." );

                await bus.Publish<IShoppingCartCheckedout>( e =>
                {
                    e.CartId = Guid.NewGuid().ToString();
                } ).ConfigureAwait(false);

                Console.WriteLine( "IShoppingCartCheckedout published." );
            }
        }

        static async Task StartDemoMode( IEndpointInstance bus ) 
        {
            while( true ) 
            {
                await PublishEvent( bus ).ConfigureAwait(false);
                await Task.Delay(5000).ConfigureAwait(false);
            }
        }
    }
}
