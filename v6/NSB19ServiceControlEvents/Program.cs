using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSB19ServiceControlEvents
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
            cfg.UseSerialization<JsonSerializer>();
            cfg.UsePersistence<InMemoryPersistence>();
            cfg.Conventions()
                .DefiningCommandsAs( t =>
                    typeof( ICommand ).IsAssignableFrom( t )
                    || ( t.Namespace != null && t.Namespace.EndsWith( ".Commands" ) ) )
                .DefiningEventsAs( t =>
                    typeof( IEvent ).IsAssignableFrom( t )
                    || ( t.Namespace != null && t.Namespace.EndsWith( ".Events" ) )
                    || ( t.Namespace != null && t.Namespace.StartsWith( "ServiceControl.Contracts" ) ) );


            var endpoint = await Endpoint.Start(cfg);

            Console.WriteLine("Endpoint is running...");
            Console.Read();

            await endpoint.Stop();

            Console.WriteLine("Endpoint stopped.");
            Console.Read();
        }
    }
}
