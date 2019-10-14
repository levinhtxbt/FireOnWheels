using FireOnWheels.MessageContracts;
using MassTransit;
using System;

namespace FireOnWheels.Registration.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            var bus = BusConfigurator.ConfigureBus((cfg, host) =>
            {
                cfg.ReceiveEndpoint(host,
                   RabbitMqConstants.RegisterOrderServiceQueue, e =>
                   {
                       e.Consumer<OrderReceivedConsumer>();
                   });
            });

            bus.Start();

            Console.WriteLine("Listening for Register order commands.. " +
                              "Press enter to exit");
            Console.ReadLine();

            bus.Stop();
        }
    }
}
