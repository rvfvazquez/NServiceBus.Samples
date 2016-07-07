using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSB11PublishIfException.Messages;
using NServiceBus;

namespace NSB11PublishIfException
{
    class FailedEventHandler: NServiceBus.IHandleMessages<Messages.FailedEvent>
    {
        public Task Handle(FailedEvent message, IMessageHandlerContext context)
        {
            Console.WriteLine("Messages.FailedEvent");

            return Task.CompletedTask;
        }
    }
}
