using FireOnWheels.MessageContracts;
using MassTransit;
using MassTransit.Saga;
using System;

namespace FireOnWheels.Saga
{
    class Program
    {
        static void Main(string[] args)
        {
            var orderSaga = new OrderSaga();
            var repo = new InMemorySagaRepository<OrderSagaState>();
            var bus = BusConfigurator.ConfigureBus((cfg, host) =>
            {
                cfg.ReceiveEndpoint(host, RabbitMqConstants.SagaQueue, e =>
                   {
                       e.PrefetchCount = 10;
                       e.StateMachineSaga(orderSaga, repo);
                   });
            });

            bus.Start();

            Console.WriteLine("Saga active.... Press <Enter> to exit");
            Console.ReadLine();

            bus.Stop();
        }
    }
}
