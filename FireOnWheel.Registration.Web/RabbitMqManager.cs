using FireOnWheels.MessageContracts;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

namespace FireOnWheel.Registration.Web
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


        public void SendRegisterOrderCommand(IRegisterOrderCommand command)
        {
            _channel.ExchangeDeclare(
                exchange: RabbitMqConstants.RegisterOrderExchange,
                type: ExchangeType.Direct);
            _channel.QueueDeclare(
                queue: RabbitMqConstants.RegisterOrderQueue,
                durable: false,
                exclusive: false,
                autoDelete: true,
                arguments: null);
            _channel.QueueBind(queue: RabbitMqConstants.RegisterOrderQueue,
                exchange: RabbitMqConstants.RegisterOrderExchange,
                routingKey: "");

            var messageProperties = _channel.CreateBasicProperties();
            messageProperties.ContentType = RabbitMqConstants.JsonMimeType;

            var serializedCommand = JsonSerializer.Serialize(command);

            _channel.BasicPublish(RabbitMqConstants.RegisterOrderExchange,
                routingKey: "",
                basicProperties: messageProperties,
                body: Encoding.UTF8.GetBytes(serializedCommand));

        }

        public void Dispose()
        {
            if (!_channel.IsClosed)
                _channel.Close();
        }
    }
}
