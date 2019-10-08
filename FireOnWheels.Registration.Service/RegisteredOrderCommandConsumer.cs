﻿using FireOnWheels.MessageContracts;
using FireOnWheels.Registration.Service.Messages;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

namespace FireOnWheels.Registration.Service
{
    public class RegisteredOrderCommandConsumer : DefaultBasicConsumer
    {
        private readonly RabbitMqManager rabbitMqManager;

        public RegisteredOrderCommandConsumer(
            RabbitMqManager rabbitMqManager)
        {
            this.rabbitMqManager = rabbitMqManager;
        }

        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, byte[] body)
        {
            //base.HandleBasicDeliver(consumerTag, deliveryTag, redelivered, exchange, routingKey, properties, body);

            if (properties.ContentType != RabbitMqConstants.JsonMimeType)
                throw new ArgumentException(
                    $"Can't handle content type {properties.ContentType}");

            var message = Encoding.UTF8.GetString(body);
            var commandObj =
                JsonSerializer.Deserialize<RegisterOrderCommand>(
                    message);

            Consume(commandObj);
            rabbitMqManager.SendAck(deliveryTag);
        }

        private void Consume(IRegisterOrderCommand command)
        {
            //Store order registration and get Id
            var id = 12;

            Console.WriteLine($"Order with id {id} registered");
            Console.WriteLine("Publishing order registered event");

            //notify subscribers that a order is registered
            var orderRegisteredEvent = new OrderRegisteredEvent(command, id);
            //publish event
            rabbitMqManager.SendOrderRegisteredEvent(orderRegisteredEvent);
        }
    }
}
