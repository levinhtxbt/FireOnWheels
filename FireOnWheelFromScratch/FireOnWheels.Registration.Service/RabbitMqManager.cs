using FireOnWheels.MessageContracts;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

namespace FireOnWheels.Registration.Service
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

        public void ListenForRegisterOrderCommand()
        {
            _channel.QueueDeclare(queue: RabbitMqConstants.RegisterOrderQueue,
                durable: false,
                exclusive: false,
                autoDelete: true,
                arguments: null);
            _channel.BasicQos(
                prefetchSize: 0, 
                prefetchCount: 1, 
                global: false);
            var consumer = new RegisteredOrderCommandConsumer(this);

            _channel.BasicConsume(
                queue: RabbitMqConstants.RegisterOrderQueue,
                autoAck: false,
                consumer: consumer);
        }

        public void SendOrderRegisteredEvent(IOrderRegisteredEvent command)
        {
            _channel.ExchangeDeclare(
                exchange: RabbitMqConstants.OrderRegisteredExchange, 
                type: ExchangeType.Direct);
            _channel.QueueDeclare(
                queue: RabbitMqConstants.OrderRegisteredNotificationQueue,
                durable: false,
                exclusive: false,
                autoDelete: true,
                arguments: null);
            _channel.QueueBind(
                queue: RabbitMqConstants.OrderRegisteredNotificationQueue,
                exchange: RabbitMqConstants.OrderRegisteredExchange,
                routingKey: "",
                arguments: null);

            var serilizedCommand = JsonSerializer.Serialize(command);

            var messageProperties = _channel.CreateBasicProperties();
            messageProperties.ContentType = RabbitMqConstants.JsonMimeType;

            _channel.BasicPublish(
                exchange: RabbitMqConstants.OrderRegisteredExchange,
                routingKey: "",
                basicProperties: messageProperties,
                body: Encoding.UTF8.GetBytes(serilizedCommand));

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
