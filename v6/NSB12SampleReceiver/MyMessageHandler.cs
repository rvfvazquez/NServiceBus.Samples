using NSB12SampleMessages;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical;

namespace NSB12SampleReceiver
{
    class MyMessageHandler : IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            using(ConsoleColor.Cyan.AsForegroundColor())
            {
                Console.WriteLine("Sending MyReply to:  {0}", context.ReplyToAddress);

                var reply = new MyReply()
                {
                    Content = "How you doing?"
                };

                await context.Reply(reply).ConfigureAwait(false);

                Console.WriteLine("Reply sent.");
            }
        }
    }
}
