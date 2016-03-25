using NServiceBus;
using NServiceBus.Persistence;
using Raven.Client.Embedded;
using System;
using Topics.Radical.Helpers;

namespace NSB14CustomerCare
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
                .UsingNames( "CustomerCare", Environment.MachineName );

            var embeddedSore = new EmbeddableDocumentStore
            {
                ResourceManagerId = new Guid( "{6B8BF798-24D2-402E-AED2-0F3D801571A0}" ),
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
                Console.Read();
            }
        }
    }
}
