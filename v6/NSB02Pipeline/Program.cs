using NServiceBus;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;
using System;
using System.Threading.Tasks;

namespace NSB02Pipeline
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            var cfg = new EndpointConfiguration(typeof(Program).Namespace);
            cfg.EnableInstallers();

            cfg.UsePersistence<InMemoryPersistence>();
            cfg.Conventions()
                .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith(".Commands"))
                .DefiningEventsAs(t => t.Namespace != null && t.Namespace.EndsWith(".Events"));

            cfg.Pipeline.Register("MyStep", typeof(MyBehavior), "behavior description");
            cfg.Pipeline.Register<MyOtherBehaviorSetup>();

            var endpoint = await Endpoint.Start(cfg);

            Console.Read();

            await endpoint.Stop();
        }
    }

    class MyOtherBehaviorSetup : RegisterStep
    {
        public MyOtherBehaviorSetup()
            : base("MyOtherBehavior", typeof(MyOtherBehavior), "description here...")
        {
            InsertAfter(WellKnownStep.DeserializeMessages);
        }
    }


    class MyBehavior : Behavior<IncomingContext>
    {
        public void Invoke(IncomingContext context, System.Action next)
        {
            //can do something before moving on

            //context.IncomingLogicalMessage;
            //context.LogicalMessages;
            //context.MessageHandler;
            //context.PhysicalMessage;
            //context.Builder;
            //context.DoNotInvokeAnyMoreHandlers();

            next();

            //can do something after the message has been handled
        }
    }


    class MyOtherBehavior : Behavior<IncomingContext>
    {
        public void Invoke(IncomingContext context, System.Action next)
        {
            //can do something before moving on

            next();

            //can do something after the message has been handled
        }
    }

}
