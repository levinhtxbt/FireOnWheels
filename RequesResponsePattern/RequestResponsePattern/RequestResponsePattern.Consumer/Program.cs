using MassTransit;
using System;

namespace RequestResponsePattern.Consumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var bus = BusConfigurator.ConfigureBus((cfg, host) =>
            {
                cfg.ReceiveEndpoint(host, RabbitMqConstants.NotificationServiceQueue, e =>
                {
                    e.Consumer<ReceiveMessageConsumer>();
                });
            });

            bus.Start();

            Console.WriteLine("Listening message......");
            Console.ReadKey();

            bus.Stop();

        }
    }
}
