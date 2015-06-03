
namespace NSB11ManualSubscribePublisher
{
    using NServiceBus;

	/*
		This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
		can be found here: http://particular.net/articles/the-nservicebus-host
	*/
	public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
    {
    }

    public class CustomInitialization : INeedInitialization
    {
        public void Init()
        {
            Configure.Instance.DefiningCommandsAs( t => t.Namespace != null && t.Namespace.EndsWith( ".Commands" ) );
            Configure.Instance.DefiningEventsAs( t => t.Namespace != null && t.Namespace.EndsWith( ".Events" ) );

            Configure.Instance.DisableGateway();

            Configure.Instance
                .UseNHibernateSubscriptionPersister()
                .UseNHibernateTimeoutPersister()
                .UseNHibernateSagaPersister();
        }
    }
}
