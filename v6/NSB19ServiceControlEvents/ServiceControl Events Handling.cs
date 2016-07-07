using NServiceBus;
using ServiceControl.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical;

namespace NSB19ServiceControlEvents
{
    class MessageFailedHandler : IHandleMessages<MessageFailed>
    {
        public Task Handle(MessageFailed message, IMessageHandlerContext context)
        {
            using(ConsoleColor.Red.AsForegroundColor())
            {
                Console.WriteLine("Message failed at {0} for {1} time(s) due to '{2}'",
                    message.ProcessingEndpoint.Name,
                    message.NumberOfProcessingAttempts,
                    message.FailureDetails.Exception.Message);
            }

            return Task.CompletedTask;
        }
    }

    class HeartbeatStoppedHandler : IHandleMessages<HeartbeatStopped>
    {
        public Task Handle( HeartbeatStopped message, IMessageHandlerContext context)
        {
            using( ConsoleColor.Red.AsForegroundColor() )
            {
                Console.WriteLine( "Heartbeat from {0} stopped.", message.EndpointName );
            }

            return Task.CompletedTask;
        }
    }

    class HeartbeatRestoredHandler : IHandleMessages<HeartbeatRestored>
    {
        public Task Handle( HeartbeatRestored message, IMessageHandlerContext context)
        {
            using( ConsoleColor.Green.AsForegroundColor() )
            {
                Console.WriteLine( "Heartbeat from {0} restored.", message.EndpointName );
            }

            return Task.CompletedTask;
        }
    }

    class CustomCheckSucceededHandler : IHandleMessages<CustomCheckSucceeded>
    {
        public Task Handle( CustomCheckSucceeded message, IMessageHandlerContext context)
        {
            using( ConsoleColor.Green.AsForegroundColor() )
            {
                Console.WriteLine( "CustomCheck {0} from {1} succeeded.", message.Category, message.EndpointName );
            }

            return Task.CompletedTask;
        }
    }

    class CustomCheckFailedHandler : IHandleMessages<CustomCheckFailed>
    {
        public Task Handle( CustomCheckFailed message, IMessageHandlerContext context)
        {
            using( ConsoleColor.Red.AsForegroundColor() )
            {
                Console.WriteLine( "CustomCheck {0} from {1} failed.", message.Category, message.EndpointName );
            }

            return Task.CompletedTask;
        }
    }
}