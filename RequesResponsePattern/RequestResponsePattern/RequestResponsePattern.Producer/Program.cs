using MassTransit;
using System;

namespace RequestResponsePattern.Producer
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {

            var bus = BusConfigurator.ConfigureBus();
            var address = new Uri($"{RabbitMqConstants.RabbitMqUri}{RabbitMqConstants.NotificationServiceQueue}");
            var timeout = TimeSpan.FromSeconds(10);

            var requestClient = bus.CreateRequestClient<IRequestMessage>(
                address);

            bus.Start();

            Console.WriteLine("The message has been sent");

            var result = await requestClient.GetResponse<IResponseMessage>(new RequestMessage
            {
                Id = 1,
                Content = "hello fucker"
            });

            Console.WriteLine(result.Message.Content);

            Console.ReadKey();

            //var client = new MessageRequestClient<RequestMessage, ResponseMessage>(bus,
            //    address, timeout, timeout);

        }
    }
}
