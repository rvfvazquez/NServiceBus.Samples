using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace NSB18HandlersSortOrder.Handlers
{
    class ValidationHandler : NServiceBus.IHandleMessages<Object>
    {
        public Task Handle(object message, IMessageHandlerContext context)
        {
            return Task.CompletedTask;
        }
    }
}
