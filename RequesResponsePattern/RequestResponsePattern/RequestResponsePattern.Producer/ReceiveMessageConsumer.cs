using MassTransit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RequestResponsePattern.Producer
{
    public class ReceiveMessageConsumer : IConsumer<IRequestMessage>
    {
        public async Task Consume(ConsumeContext<IRequestMessage> context)
        {
           
            Console.WriteLine($"Receive a message {context.Message.Id} with content: {context.Message.Content}");

            await context.RespondAsync<IResponseMessage>(new ResponseMessage
            {
                Id = 2,
                Content = "Hello fucker again"
            });

            Console.WriteLine("Response message successfully!!!");
        }
    }
}
