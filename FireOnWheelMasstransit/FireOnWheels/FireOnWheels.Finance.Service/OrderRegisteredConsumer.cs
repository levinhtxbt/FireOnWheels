using FireOnWheels.MessageContracts;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FireOnWheels.Finance.Service
{
    class OrderRegisteredConsumer : IConsumer<IOrderRegisteredEvent>
    {
        public async Task Consume(ConsumeContext<IOrderRegisteredEvent> context)
        {
            //Save to db
            await Console.Out.WriteLineAsync($"New order received: Order id {context.Message.OrderId}");
        }
    }
}
