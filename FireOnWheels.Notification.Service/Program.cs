using System;

namespace FireOnWheels.Notification.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Notification service";
            using (var rabbitMqManager = new RabbitMqManager())
            {
                rabbitMqManager.ListenForOrderRegisteredEvent();
                Console.WriteLine("Listening for OrderRegisteredEvent..");
                Console.ReadKey();
            }
        }
    }
}
