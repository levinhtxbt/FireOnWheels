using System;
using System.Collections.Generic;
using System.Text;

namespace RequestResponsePattern
{
    public static class RabbitMqConstants
    {
        public const string RabbitMqUri = "rabbitmq://localhost/";
        public const string UserName = "guest";
        public const string Password = "guest";

        public const string RegisterOrderServiceQueue = "registerorder.service";
        public const string NotificationServiceQueue = "notification.service";
        public const string FinanceServiceQueue = "finance.service";
    }
}
