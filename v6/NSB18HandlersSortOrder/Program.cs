using NSB18HandlersSortOrder.Handlers;
using NServiceBus;
using NServiceBus.Persistence;
using Raven.Client.Embedded;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSB18HandlersSortOrder
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            var cfg = new EndpointConfiguration(typeof(Program).Namespace);
            cfg.UniquelyIdentifyRunningInstance()
                .UsingCustomIdentifier(new Guid("{41E0F4CE-A4AE-4A30-8F78-B4CC3E912ABD}"));

            cfg.EnableInstallers();

            var embeddedSore = new EmbeddableDocumentStore
            {
                ResourceManagerId = new Guid("{46FFEA87-77A7-43A7-88D3-79778E677D52}"),
                DataDirectory = @"~\RavenDB\Data"
            }.Initialize();

            cfg.UsePersistence<RavenDBPersistence>()
                .DoNotSetupDatabasePermissions()
                .SetDefaultDocumentStore(embeddedSore);

            cfg.Conventions()
                .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith(".Commands"))
                .DefiningEventsAs(t => t.Namespace != null && t.Namespace.EndsWith(".Events"));

            cfg.ExecuteTheseHandlersFirst(
                typeof(SecurityHandler),
                typeof(ValidationHandler)
            );

            var endpoint = await Endpoint.Start(cfg);

            await endpoint.SendLocal(new Commands.StartSagaCommand()
            {
                Sample = "Hi, there!"
            });

            Console.Read();

            await endpoint.Stop();
        }
    }
}
