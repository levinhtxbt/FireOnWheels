using FireOnWheels.MessageContracts;
using RabbitMQ.Client;
using System;

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
