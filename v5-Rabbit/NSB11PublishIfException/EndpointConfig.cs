
namespace NSB11PublishIfException
{
    using NServiceBus;

    /*
        This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
        can be found here: http://particular.net/articles/the-nservicebus-host
    */
    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration configuration)
        {
            configuration.UsePersistence<InMemoryPersistence>();
            configuration.UseSerialization<JsonSerializer>();
            configuration.UseTransport<RabbitMQTransport>()
                .ConnectionString("host=localhost");
        }
    }

    class Runner : IWantToRunWhenBusStartsAndStops
    {
        public IBus Bus { get; set; }

        public void Start()
        {
            this.Bus.Send( new Messages.PoisonMessage() );
        }

        public void Stop()
        {
            
        }
    }

}
