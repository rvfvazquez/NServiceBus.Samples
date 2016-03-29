using NServiceBus;
using System;
using System.Threading.Tasks;

namespace NSB20ManualSubscriptions.Publisher
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

            cfg.UsePersistence<InMemoryPersistence>();
            cfg.Conventions()
                .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith(".Commands"))
                .DefiningEventsAs(t => t.Namespace != null && t.Namespace.EndsWith(".Events"));

            var endpoint = await Endpoint.Start(cfg).ConfigureAwait(false);

            Console.Read();

            await endpoint.Stop().ConfigureAwait(false);
        }
    }
}
