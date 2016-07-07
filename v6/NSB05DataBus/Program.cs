
namespace NSB05DataBus
{
    using Commands;
    using NServiceBus;
    using System;
    using System.Threading.Tasks;
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            var cfg = new EndpointConfiguration(typeof(Program).Namespace);
            cfg.EnableInstallers();
            cfg.UsePersistence<InMemoryPersistence>();
            cfg.Conventions()
                .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith(".Commands"));

            var endpoint = await Endpoint.Start(cfg).ConfigureAwait(true);

            Console.WriteLine("Endpoint is running...");

            var message = new MessageWithLargePayload
            {
                SomeProperty = "This message contains a large blob that will be sent on the data bus",
                LargeBlob = new byte[ 1024 * 1024 * 5 ] //5MB
                //LargeBlob = new byte[ 1024 * 1024 * 900 ] //900MB
            };

            await endpoint.SendLocal(message).ConfigureAwait(false);

            Console.WriteLine("Large message sent...");

            Console.Read();

            await endpoint.Stop().ConfigureAwait(true);

            Console.WriteLine("Disposed");
            Console.Read();
        }
    }
}
