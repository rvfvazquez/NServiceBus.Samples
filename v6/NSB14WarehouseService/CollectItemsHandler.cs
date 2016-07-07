using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSB14WarehouseService.Messages.Commands;
using NSB14WarehouseService.Messages.Events;
using NServiceBus;
using Topics.Radical;
using Topics.Radical.Helpers;
using System.Threading;

namespace NSB14WarehouseService
{
    class CollectItemsHandler : IHandleMessages<CollectItems>
    {
        public IBus Bus { get; set; }

        public void Handle( CollectItems message )
        {
            if( Program.IsDemoMode )
            {
                int rem;
                Math.DivRem( DateTime.Now.Minute, 2, out rem );
                if( rem == 0 )
                {
                    using( ConsoleColor.Yellow.AsForegroundColor() )
                    {
                        Console.WriteLine( "Something is slowing down the collection process for order: " + message.OrderId );
                        Thread.Sleep( 6000 );
                    }
                }
            }

            using( ConsoleColor.Magenta.AsForegroundColor() )
            {
                Console.WriteLine( "Collect items request for cart: {0}", message.CartId );

                this.Bus.Publish<IItemsCollected>( e =>
                {
                    e.OrderId = message.OrderId;
                } );

                Console.WriteLine( "Items collected." );
            }
        }
    }
}
