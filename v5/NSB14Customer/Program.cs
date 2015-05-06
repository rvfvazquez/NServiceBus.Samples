using Topics.Radical;
using NServiceBus;
using NServiceBus.Persistence;
using Raven.Client.Embedded;
using System;
using System.Linq;
using NSB14Customer.Messages.Events;

namespace NSB14Customer
{
	class Program
	{
		static IBus bus;

		static void Main( string[] args )
		{
			var cfg = new BusConfiguration();

			var embeddedSore = new EmbeddableDocumentStore
			{
				ResourceManagerId = new Guid( "{B9EC41C8-6DAB-4EF2-9805-9181F0A8B208}" ),
				DataDirectory = @"~\RavenDB\Data"
			}.Initialize();

			cfg.UsePersistence<RavenDBPersistence>()
				.DoNotSetupDatabasePermissions()
				.SetDefaultDocumentStore( embeddedSore );

			cfg.Conventions()
				.DefiningCommandsAs( t => t.Namespace != null && t.Namespace.EndsWith( ".Commands" ) )
				.DefiningEventsAs( t => t.Namespace != null && t.Namespace.EndsWith( ".Events" ) );

			bus = Bus.Create( cfg ).Start();
			using( ConsoleColor.Red.AsForegroundColor() )
			{
				Console.WriteLine( "Isn't this an amazing web site? :-D" );
			}

			Execute();
		}

		static ConsoleKey Ask()
		{
			Console.WriteLine();
			Console.WriteLine();

			var validResponses = new[] { ConsoleKey.S, ConsoleKey.C, ConsoleKey.E };
			using( ConsoleColor.Green.AsForegroundColor() )
			{
				Console.WriteLine( "Press S to start a new order, or press X to exit." );
			}
			var response = Console.ReadKey().Key;

			if( validResponses.Any( vr => vr == response ) )
			{
				Console.WriteLine();
				Console.WriteLine();

				return response;
			}

			Console.WriteLine( "unknown..." );
			return Ask();
		}

		static void Execute()
		{
			var action = Ask();
			if( action == ConsoleKey.S )
			{
				using( ConsoleColor.Cyan.AsForegroundColor() )
				{
					Console.WriteLine( "Publishing IShoppingCartCheckedout..." );

					bus.Publish<IShoppingCartCheckedout>( e =>
					{
						e.CartId = Guid.NewGuid().ToString();
					} );

					Console.WriteLine( "IShoppingCartCheckedout published." );
				}

				Execute();
			}
			else if( action == ConsoleKey.X )
			{
				Console.WriteLine( "\tClosing..." );
			}
		}
	}
}
