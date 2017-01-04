
using NServiceBus;
using NServiceBus.Persistence;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using System;
using Topics.Radical.Helpers;
namespace NSB14OrderManager
{
    class Program
    {
        public static Boolean IsDemoMode { get; private set; }

        static void Main( string[] args )
        {
            var cmdLine = CommandLine.GetCurrent();
            Program.IsDemoMode = cmdLine.Contains( "demo" );

            var cfg = new BusConfiguration();
            cfg.EnableInstallers();
            cfg.UniquelyIdentifyRunningInstance()
                .UsingNames( "OrderManager", Environment.MachineName );

            cfg.EnableInstallers();

            var embeddedSore = new EmbeddableDocumentStore
            {
                ResourceManagerId = new Guid( "{46FFEA87-77A7-43A7-88D3-79778E677D52}" ),
                DataDirectory = @"~\RavenDB\Data"
            }.Initialize();
            new RavenDocumentsByEntityName().Execute(embeddedSore);

            cfg.UsePersistence<RavenDBPersistence>()
                .DoNotSetupDatabasePermissions()
                .SetDefaultDocumentStore( embeddedSore );

            cfg.Conventions()
                .DefiningCommandsAs( t => t.Namespace != null && t.Namespace.EndsWith( ".Commands" ) )
                .DefiningEventsAs( t => t.Namespace != null && t.Namespace.EndsWith( ".Events" ) );

            using( var bus = Bus.Create( cfg ).Start() )
            {
                Console.Read();
            }
        }
    }
}
