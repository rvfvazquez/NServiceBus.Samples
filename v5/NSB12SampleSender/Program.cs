using NSB12SampleMessages;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical;

namespace NSB12SampleSender
{
	class Program
	{
		static void Main( string[] args )
		{
			var cfg = new BusConfiguration();

			cfg.UsePersistence<InMemoryPersistence>();
			cfg.Conventions()
				.DefiningMessagesAs( t => t.Namespace != null && t.Namespace.EndsWith( "Messages" ) );

			using( var bus = Bus.Create( cfg ).Start() )
			{
				Logic.Run( setup =>
				{
					setup.DefineAction( ConsoleKey.S, "Sends a new message.", () =>
					{
						bus.Send( new MyMessage()
						{
							Content = "this is from sender :-)"
						} );
					} );
				} );
			}
		}
	}
}
