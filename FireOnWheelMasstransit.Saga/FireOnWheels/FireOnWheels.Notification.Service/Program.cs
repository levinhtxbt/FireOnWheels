using FireOnWheels.MessageContracts;
using MassTransit;
using System;

namespace FireOnWheels.Notification.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            var bus = BusConfigurator.ConfigureBus((cfg, host) =>
            {
                cfg.ReceiveEndpoint(RabbitMqConstants.NotificationServiceQueue, e =>
                {
                    e.Handler<IOrderRegisteredEvent>(c =>
                        {
                            return Console.Out.WriteLineAsync($"Customer notification sent: " + $"Order id {c.Message.OrderId}");
                        });

                });
            });

            bus.Start();

            Console.WriteLine("Listening for Order registered events.. Press enter to exit");
            Console.ReadLine();

            bus.Stop();


        }
    }
}
