using NServiceBus;
using NServiceBus.Persistence;
using Raven.Client.Embedded;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSB18HandlersSortOrder
{
	class Program
	{
		static void Main( string[] args )
		{
			var cfg = new BusConfiguration();
			cfg.EnableInstallers();

			var embeddedSore = new EmbeddableDocumentStore
			{
				ResourceManagerId = new Guid( "{46FFEA87-77A7-43A7-88D3-79778E677D52}" ),
				DataDirectory = @"~\RavenDB\Data"
			}.Initialize();

			cfg.UsePersistence<RavenDBPersistence>()
				.DoNotSetupDatabasePermissions()
				.SetDefaultDocumentStore( embeddedSore );

			cfg.Conventions()
				.DefiningCommandsAs( t => t.Namespace != null && t.Namespace.EndsWith( ".Commands" ) )
				.DefiningEventsAs( t => t.Namespace != null && t.Namespace.EndsWith( ".Events" ) );

			cfg.LoadMessageHandlers( 
				First<Handlers.SecurityHandler>
					.Then<Handlers.ValidationHandler>()
			);

			using( var bus = Bus.Create( cfg ).Start() )
			{
				bus.SendLocal( new Commands.StartSagaCommand()
				{
					Sample = "Hi, there!"
				} );

				Console.Read();
			}
		}
	}
}
