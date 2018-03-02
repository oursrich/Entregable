using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using System.Threading;

namespace ReceiveMessages
{
    public class ReceiveMessage
    {
        
        public void Receive(IConnection connection, string queue)
        {
            
            try
            {
                using (var channel = connection.CreateModel())
                {
                    // This instructs the channel not to prefetch more than one message
                    channel.BasicQos(0, 1, false);

                    // Create a new, durable exchange
                    channel.ExchangeDeclare("Respuesta", ExchangeType.Direct, true, false, null);
                    // Create a new, durable queue
                    channel.QueueDeclare("ColasRespuesta", true, false, false, null);
                    // Bind the queue to the exchange
                    channel.QueueBind("ColasRespuesta", "Respuesta", "optional-routing-key");

                    using (var subscription = new Subscription(channel, "ColasRespuesta", false))
                    {
                        Console.WriteLine("Waiting for messages...");
                        var encoding = new UTF8Encoding();
                        while (channel.IsOpen)
                        {
                            BasicDeliverEventArgs eventArgs;
                            var success = subscription.Next(2000, out eventArgs);
                            if (success == false) continue;
                            var msgBytes = eventArgs.Body;
                            var message = encoding.GetString(msgBytes);
                            Console.WriteLine(message);
                            channel.BasicAck(eventArgs.DeliveryTag, false);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }



    }
}
