using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSB14ShippingService.Messages.Commands;
using NSB14ShippingService.Messages.Events;
using NServiceBus;
using Topics.Radical;
using Topics.Radical.Helpers;

namespace NSB14ShippingService
{
	class ShipOrderHandler : NServiceBus.IHandleMessages<ShipOrder>
	{
		public IBus Bus { get; set; }

		public void Handle( ShipOrder message )
		{
            if( Program.IsDemoMode )
            {
                int rem;
                Math.DivRem( DateTime.Now.Minute, 2, out rem );
                if( rem == 0 )
                {
                    throw new Exception( "Some randomly evil code..." );
                }
            }

			using( ConsoleColor.Cyan.AsForegroundColor() )
			{
				Console.WriteLine( "Shipping order {0}...", message.OrderId );

				this.Bus.Publish<IOrderShipped>( e =>
				{
					e.OrderId = message.OrderId;
				} );

				Console.WriteLine( "Order shipped." );
			}
		}
	}
}
