using NServiceBus;
using ServiceControl.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical;

namespace NSB19ServiceControlEvents
{
	class MessageFailedHandler : IHandleMessages<MessageFailed>
	{
		public void Handle( MessageFailed message )
		{
			using( ConsoleColor.Red.AsForegroundColor() )
			{
				Console.WriteLine( "Message failed at {0} for {1} time(s) due to '{2}'",
					message.ProcessingEndpoint.Name,
					message.NumberOfProcessingAttempts,
					message.FailureDetails.Exception.Message );
			}
		}
	}

	class HeartbeatStoppedHandler : IHandleMessages<HeartbeatStopped>
	{
		public void Handle( HeartbeatStopped message )
		{
			using( ConsoleColor.Red.AsForegroundColor() )
			{
				Console.WriteLine( "Heartbeat from {0} stopped.", message.EndpointName );
			}
		}
	}

	class HeartbeatRestoredHandler : IHandleMessages<HeartbeatRestored>
	{
		public void Handle( HeartbeatRestored message )
		{
			using( ConsoleColor.Green.AsForegroundColor() )
			{
				Console.WriteLine( "Heartbeat from {0} restored.", message.EndpointName );
			}
		}
	}

	class CustomCheckSucceededHandler : IHandleMessages<CustomCheckSucceeded>
	{
		public void Handle( CustomCheckSucceeded message )
		{
			using( ConsoleColor.Green.AsForegroundColor() )
			{
				Console.WriteLine( "CustomCheck {0} from {1} succeeded.", message.Category, message.EndpointName );
			}
		}
	}

	class CustomCheckFailedHandler : IHandleMessages<CustomCheckFailed>
	{
		public void Handle( CustomCheckFailed message )
		{
			using( ConsoleColor.Red.AsForegroundColor() )
			{
				Console.WriteLine( "CustomCheck {0} from {1} failed.", message.Category, message.EndpointName );
			}
		}
	}
}