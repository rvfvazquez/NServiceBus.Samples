using NServiceBus;
using ServiceControl.Plugin.CustomChecks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSB09CustomChecks
{
    class Program
    {
        static void Main( string[] args )
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            var cfg = new EndpointConfiguration(typeof(Program).Namespace);
            cfg.EnableInstallers();
            cfg.UsePersistence<InMemoryPersistence>();
            cfg.Conventions()
                .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith(".Commands"));

            //using( var bus = Bus.CreateSendOnly( cfg ) )
            //{
            //    Console.WriteLine("Send-only bus is running...");
            //    Console.Read();
            //}

            var endpoint = await Endpoint.Start(cfg);

            Console.WriteLine("Endpoint is running...");
            Console.Read();

            await endpoint.Stop();

            Console.WriteLine("Disposed");
            Console.Read();
        }
    }

    class MyCheck : CustomCheck
    {
        public MyCheck()
            : base( "Directory check", "My Server", TimeSpan.FromSeconds( 5 ) ) { }

        public override Task<CheckResult> PerformCheck()
        {
            return Task.Run(() => 
            {
                var dir = @"C:\Foo";
                if(!Directory.Exists(dir))
                {
                    return CheckResult.Failed(string.Format("Storage directory '{0}' does not exist", dir));
                }

                return CheckResult.Pass;
            });
        }
    }
}
