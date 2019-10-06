using FireOnWheels.MessageContracts;
using System;

namespace FireOnWheels.Notification.Service
{
    public class OrderRegisteredConsumer
    {
        public void Consume(IOrderRegisteredEvent registeredEvent)
        {
            //Send notification to user
            Console.WriteLine($"Customer notification sent: Order id {registeredEvent.OrderId} registered");
        }
    }
}
