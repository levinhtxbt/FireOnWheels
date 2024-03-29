﻿using FireOnWheels.MessageContracts;
using FireOnWheels.Registration.Service.Messages;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FireOnWheels.Registration.Service
{
    public class RegisterOrderCommandConsumer : IConsumer<IRegisterOrderCommand>
    {
        public async Task Consume(ConsumeContext<IRegisterOrderCommand> context)
        {
            var command = context.Message;

            //Store order registration and get Id
            var id = 12;

            await Console.Out.WriteLineAsync($"Order with id {id} registered");

            //notify subscribers that a order is registered
            var orderRegisteredEvent = new OrderRegisteredEvent(command, id);
            //publish event
            await context.Publish<IOrderRegisteredEvent>(orderRegisteredEvent);
        }
    }
}
