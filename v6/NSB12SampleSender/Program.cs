using NSB12SampleMessages;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical;

namespace NSB12SampleSender
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

            cfg.UsePersistence<InMemoryPersistence>();

            cfg.Conventions()
                .DefiningMessagesAs( t => t.Namespace != null && t.Namespace.EndsWith( "Messages" ) );

            var endpoint = await Endpoint.Start(cfg).ConfigureAwait(false);

            Logic.Run(setup =>
            {
                setup.DefineAction(ConsoleKey.S, "Sends a new message.", async () =>
                {
                    await endpoint.Send(new MyMessage()
                    {
                        Content = "this is from sender :-)"
                    }).ConfigureAwait(false);
                });
            });

            await endpoint.Stop().ConfigureAwait(false);
        }
    }
}
