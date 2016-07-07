using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSB12SampleReceiver
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
            cfg.EnableInstallers();

            cfg.Conventions()
                .DefiningMessagesAs( t => t.Namespace != null && t.Namespace.EndsWith( "Messages" ) );

            var endpoint = await Endpoint.Start(cfg).ConfigureAwait(false);

            Console.Read();

            await endpoint.Stop().ConfigureAwait(false);
        }
    }
}
