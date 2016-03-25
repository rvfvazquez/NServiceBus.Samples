using Topics.Radical;
using NServiceBus;
using NServiceBus.Persistence;
using Raven.Client.Embedded;
using System;
using System.Linq;
using NSB14Customer.Messages.Events;
using Topics.Radical.Helpers;
using System.Threading;

namespace NSB14Customer
{
    class Program
    {
        public static Boolean IsDemoMode { get; private set; }

        static void Main( string[] args )
        {
            var cmdLine = CommandLine.GetCurrent();
            Program.IsDemoMode = cmdLine.Contains( "demo" );

            var cfg = new BusConfiguration();

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

            using( var bus = Bus.Create( cfg ).Start() )
            {
                using( ConsoleColor.Red.AsForegroundColor() )
                {
                    Console.WriteLine( "Isn't this an amazing web site? :-D" );
                }

                if( Program.IsDemoMode )
                {
                    StartDemoMode( bus );
                }
                else
                {
                    Logic.Run( setup =>
                    {
                        setup.DefineAction( ConsoleKey.C, "Checkout", () =>
                        {
                            PublishEvent( bus );
                        } );
                    } );
                }
            }
        }

        static void PublishEvent( IBus bus ) 
        {
            using( ConsoleColor.Cyan.AsForegroundColor() )
            {
                Console.WriteLine( "Publishing IShoppingCartCheckedout..." );

                bus.Publish<IShoppingCartCheckedout>( e =>
                {
                    e.CartId = Guid.NewGuid().ToString();
                } );

                Console.WriteLine( "IShoppingCartCheckedout published." );
            }
        }

        static void StartDemoMode( IBus bus ) 
        {
            while( true ) 
            {
                PublishEvent( bus );
                Thread.Sleep( 5000 );
            }
        }
    }
}
