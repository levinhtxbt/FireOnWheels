using System;
using System.Collections.Generic;
using System.Text;

namespace FireOnWheels.MessageContracts
{
    public static class RabbitMqConstants
    {
        public const string RabbitMqUri = "rabbitmq://localhost/fireonwheels/";
        public const string UserName = "guest";
        public const string Password = "guest";
        public const string RegisterOrderServiceQueue = "registerorder.service";
        public const string NotificationServiceQueue = "notification.service";
        public const string FinanceServiceQueue = "finance.service";
        public const string SagaQueue = "saga.service";
    }
}
