
namespace NSB06Retries.Receiver
{
    using NServiceBus;
    using System;
    /*
        This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
        can be found here: http://particular.net/articles/the-nservicebus-host
    */
    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration configuration)
        {
            configuration.UsePersistence<InMemoryPersistence>();
            configuration.EnableInstallers();
            configuration.Conventions()
                .DefiningCommandsAs( t => t.Namespace != null && t.Namespace.EndsWith( ".Commands" ) )
                .DefiningEventsAs( t => t.Namespace != null && t.Namespace.EndsWith( ".Events" ) );

            configuration.SecondLevelRetries()
                .CustomRetryPolicy(transportMessage => 
                {
                    var t = transportMessage.ExceptionType();

                    if(transportMessage.NumberOfRetries() >= 3)
                    {
                        // sending back a TimeSpan.MinValue tells the
                        // SecondLevelRetry not to retry this message
                        return TimeSpan.MinValue;
                    }

                    return TimeSpan.FromSeconds(5);
                });
        }
    }

    static class ErrorsHeadersHelper
    {

        internal static int NumberOfRetries(this TransportMessage transportMessage)
        {
            string value;
            if(transportMessage.Headers.TryGetValue(Headers.Retries, out value))
            {
                return int.Parse(value);
            }
            return 0;
        }

        internal static string ExceptionType(this TransportMessage transportMessage)
        {
            return transportMessage.Headers[ "NServiceBus.ExceptionInfo.ExceptionType" ];
        }

    }
}
