using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSB20ManualSubscriptions.Subscriber
{
    class Program
    {
        static void Main( string[] args )
        {
            var cfg = new BusConfiguration();

            cfg.UsePersistence<InMemoryPersistence>();
            cfg.Conventions()
                .DefiningCommandsAs( t => t.Namespace != null && t.Namespace.EndsWith( ".Commands" ) )
                .DefiningEventsAs( t => t.Namespace != null && t.Namespace.EndsWith( ".Events" ) );

            using( var bus = Bus.Create( cfg ).Start() )
            {
                bus.Subscribe<Messages.Events.IHaveDoneSomething>();

                Console.Read();
            }
        }
    }
}
