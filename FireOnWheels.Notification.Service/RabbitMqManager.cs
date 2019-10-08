using FireOnWheels.MessageContracts;
using FireOnWheels.Notification.Service.Messages;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;

namespace FireOnWheels.Notification.Service
{
    public class RabbitMqManager : IDisposable
    {
        private readonly IModel _channel;

        public RabbitMqManager()
        {
            var connectionFactory = new ConnectionFactory();
            connectionFactory.Uri = new Uri(RabbitMqConstants.RabbitMqUri);
            var connection = connectionFactory.CreateConnection();
            _channel = connection.CreateModel();
        }


        public void ListenForOrderRegisteredEvent()
        {
            _channel.QueueDeclare(queue: RabbitMqConstants.OrderRegisteredNotificationQueue,
                durable: false,
                exclusive: false,
                autoDelete: true,
                arguments: null);
            _channel.BasicQos(
                prefetchSize: 0,
                prefetchCount: 1,
                global: false);

            var eventing = new EventingBasicConsumer(_channel);
            eventing.Received += (chan, eventArgs) =>
            {
                var contentType = eventArgs.BasicProperties.ContentType;
                if (contentType != RabbitMqConstants.JsonMimeType)
                    throw new ArgumentException(
                        $"Can't handle content type {contentType}");

                var message = Encoding.UTF8.GetString(eventArgs.Body);
                var orderConsumer = new OrderRegisteredConsumer();
                var commandObj =
                JsonSerializer.Deserialize<OrderRegisteredEvent>(message);
                orderConsumer.Consume(commandObj);
                _channel.BasicAck(deliveryTag: eventArgs.DeliveryTag,
                    multiple: false);
            };

            _channel.BasicConsume(
                queue: RabbitMqConstants.OrderRegisteredNotificationQueue,
                autoAck: false,
                consumer: eventing);

        }

        private void Eventing_Received(object sender, BasicDeliverEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void SendAck(ulong deliveryTag)
        {
            _channel.BasicAck(deliveryTag: deliveryTag, multiple: false);
        }

        public void Dispose()
        {
            if (!_channel.IsClosed)
                _channel.Close();
        }
    }
}
