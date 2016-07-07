using NSB13SampleMessages.Events;
using NServiceBus;
using Raven.Client.Embedded;
using System;
using System.Threading.Tasks;

namespace NSB13SamplePublisher
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
            cfg.EnableInstallers();

            var embeddedSore = new EmbeddableDocumentStore
            {
                ResourceManagerId = new Guid("{FDF958EB-7EE3-42F9-B757-E9836DF1F417}"),
                DataDirectory = @"~\RavenDB\Data"
            }.Initialize();

            cfg.UsePersistence<RavenDBPersistence>()
                .DoNotSetupDatabasePermissions()
                .SetDefaultDocumentStore(embeddedSore);

            cfg.Conventions()
                .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith(".Commands"))
                .DefiningEventsAs(t => t.Namespace != null && t.Namespace.EndsWith(".Events"));

            var endpoint = await Endpoint.Start(cfg).ConfigureAwait(false);

            Logic.Run(setup =>
            {
                setup.DefineAction(ConsoleKey.P, "Publishes the event.", async () =>
                {
                    await endpoint.Publish<ISomethingHappened>(e =>
                    {
                        e.Data = "These are the event data";
                    }).ConfigureAwait(false);
                });
            });

            await endpoint.Stop().ConfigureAwait(false);
        }
    }
}