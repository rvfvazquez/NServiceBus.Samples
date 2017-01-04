﻿using NSB13SampleMessages.Events;
using NServiceBus;
using NServiceBus.Persistence;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSB13SamplePublisher
{
    class Program
    {
        static void Main( string[] args )
        {
            var cfg = new BusConfiguration();
            cfg.EnableInstallers();

            var embeddedSore = new EmbeddableDocumentStore
            {
                ResourceManagerId = new Guid( "{FDF958EB-7EE3-42F9-B757-E9836DF1F417}" ),
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
                Logic.Run( setup =>
                {
                    setup.DefineAction( ConsoleKey.P, "Publishes the event.", () =>
                    {
                        bus.Publish<ISomethingHappened>( e =>
                        {
                            e.Data = "These are the event data";
                        } );
                    } );
                } );
            }
        }
    }
}
