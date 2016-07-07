using NSB13SampleMessages.Events;
using NServiceBus.RavenDB;
using System;
using Topics.Radical;
using NServiceBus;
using System.Threading.Tasks;
using NServiceBus.RavenDB.Persistence;

namespace NSB13SampleSubscriber
{
    class SomethingHappenedHandler : NServiceBus.IHandleMessages<ISomethingHappened>
    {
        public Task Handle(ISomethingHappened message, IMessageHandlerContext context)
        {
            using(ConsoleColor.Cyan.AsForegroundColor())
            {
                Console.WriteLine("Event received, data: {0}", message.Data);
            }

            // to access current session
            var session = context.SynchronizedStorageSession.RavenSession();

            return Task.CompletedTask;
        }
    }
}
