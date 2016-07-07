using NSB05DataBus.Commands;
using System;
using System.Threading.Tasks;
using Topics.Radical;
using NServiceBus;

namespace NSB05DataBus.Handlers
{
    class MessageWithLargePayloadHandler : NServiceBus.IHandleMessages<MessageWithLargePayload>
    {
        public Task Handle(MessageWithLargePayload message, IMessageHandlerContext context)
        {
            using(ConsoleColor.Green.AsForegroundColor())
            {
                Console.WriteLine("Handled");
            }

            return Task.CompletedTask;
        }
    }
}
