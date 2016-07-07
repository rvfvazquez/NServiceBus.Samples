using NSB16WcfHosting;
using NServiceBus;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace NSB16WcfHosting
{
    class WcfHost : IWantToRunWhenBusStartsAndStops
    {
        ServiceHost host = null;

        public Task Start(IMessageSession session)
        {
            this.host = new ServiceHost(typeof(MySampleService));
            this.host.Open();

            Console.WriteLine("WCF Host started...");

            return Task.CompletedTask;
        }

        public Task Stop(IMessageSession session)
        {
            if(this.host != null && this.host.State == CommunicationState.Opened)
            {
                this.host.Close();
            }

            if(this.host != null)
            {
                ((IDisposable)this.host).Dispose();
                this.host = null;
            }

            Console.WriteLine("WCF Host stopped...");

            return Task.CompletedTask;
        }
    }
}