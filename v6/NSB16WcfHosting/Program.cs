using System;
using System.Diagnostics;
using System.ServiceProcess;
using NServiceBus;
using System.Threading.Tasks;

namespace NSB16WcfHosting
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

            cfg.SendFailedMessagesTo("error");
            cfg.UseSerialization<JsonSerializer>();
            cfg.UsePersistence<InMemoryPersistence>();

            cfg.EnableInstallers();

            var endpoint = await Endpoint.Start(cfg).ConfigureAwait(false);

            Console.Read();

            await endpoint.Stop().ConfigureAwait(false);
        }

    }
}