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
            
            connectionFactory.AutomaticRecoveryEnabled = true;
            connectionFactory.TopologyRecoveryEnabled = true;
            connectionFactory.NetworkRecoveryInterval = TimeSpan.FromSeconds(10);

            var connection = connectionFactory.CreateConnection();
            _channel = connection.CreateModel();

        }


        public void SendRegisterOrderCommand(IRegisterOrderCommand command)
        {
            _channel.ExchangeDeclare(
                exchange: RabbitMqConstants.RegisterOrderExchange,
                type: ExchangeType.Direct,
                durable: false,
                autoDelete: true);
            _channel.QueueDeclare(
                queue: RabbitMqConstants.RegisterOrderQueue,
                durable: false,
                exclusive: false,
                autoDelete: true,
                arguments: null);
            _channel.QueueBind(queue: RabbitMqConstants.RegisterOrderQueue,
                exchange: RabbitMqConstants.RegisterOrderExchange,
                routingKey: "register.order");

            _channel.ConfirmSelect();

            _channel.BasicAcks += (o, arg) =>
            {
                Console.WriteLine("======= BasicAcks");
                Console.WriteLine(o);
                Console.WriteLine(arg);

            };

            _channel.BasicNacks += (o, arg) =>
            {
                Console.WriteLine("======= BasicNAcks");
                Console.WriteLine(o);
                Console.WriteLine(arg);

            };

            var messageProperties = _channel.CreateBasicProperties();
            messageProperties.ContentType = RabbitMqConstants.JsonMimeType;

            var serializedCommand = JsonSerializer.Serialize(command);

            _channel.BasicPublish(RabbitMqConstants.RegisterOrderExchange,
                routingKey: "register.order",
                basicProperties: messageProperties,
                body: Encoding.UTF8.GetBytes(serializedCommand));

        }

        private void _channel_BasicAcks(object sender, RabbitMQ.Client.Events.BasicAckEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            if (!_channel.IsClosed)
                _channel.Close();
        }
    }
}
