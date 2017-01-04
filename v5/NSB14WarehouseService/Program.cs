
using NServiceBus;
using NServiceBus.Persistence;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using System;
using Topics.Radical.Helpers;

namespace NSB14WarehouseService
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
                .UsingNames( "Warehouse", Environment.MachineName );

            cfg.EnableInstallers();

            var embeddedSore = new EmbeddableDocumentStore
            {
                ResourceManagerId = new Guid( "d5723e19-92ad-4531-adad-8611e6e05c8a" ),
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
