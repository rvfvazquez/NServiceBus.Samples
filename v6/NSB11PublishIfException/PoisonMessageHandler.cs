using NSB11PublishIfException.Messages;
using NServiceBus;
using NServiceBus.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace NSB11PublishIfException
{
    class PoisonMessageHandler : NServiceBus.IHandleMessages<PoisonMessage>
    {
        public async Task Handle(PoisonMessage message, IMessageHandlerContext context)
        {
            var options = new PublishOptions();
            options.RequireImmediateDispatch();

            await context.Publish<FailedEvent>(e => { }, options)
                .ConfigureAwait(false);

            throw new NotImplementedException();
        }
    }
}
