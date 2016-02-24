using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSB19ServiceControlEvents
{
	class Program
	{
		static void Main( string[] args )
		{
			var cfg = new BusConfiguration();
			cfg.EnableInstallers();
			cfg.UseSerialization<JsonSerializer>();
			cfg.UsePersistence<InMemoryPersistence>();
			cfg.Conventions()
				.DefiningCommandsAs( t =>
					typeof( ICommand ).IsAssignableFrom( t )
					|| ( t.Namespace != null && t.Namespace.EndsWith( ".Commands" ) ) )
				.DefiningEventsAs( t =>
					typeof( IEvent ).IsAssignableFrom( t )
					|| ( t.Namespace != null && t.Namespace.EndsWith( ".Events" ) )
					|| ( t.Namespace != null && t.Namespace.StartsWith( "ServiceControl.Contracts" ) ) );

			using( var bus = Bus.Create( cfg ).Start() )
			{
				Console.WriteLine( "Bus is running..." );
				Console.Read();
			}

			Console.WriteLine( "Disposed" );
			Console.Read();
		}
	}
}
