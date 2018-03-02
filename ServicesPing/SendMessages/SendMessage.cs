using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SendMessages
{
    public class SendMessage
    {

      
        
        public void Send(IConnection connection,string queue)
        {
            string data = "Enviando Servicios";
            
            try
            {
                using (var channel = connection.CreateModel())
                {
                   
                    channel.ExchangeDeclare("Servicios", ExchangeType.Direct, true, false, null);
                  
                    channel.QueueDeclare("Colas", true, false, false, null);
                   
                    channel.QueueBind("Colas", "Servicios", "optional-routing-key");
                   

                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 1;
                    properties.ClearMessageId();
                    properties.ReplyTo= "Servicios";
                    var encoding = new UTF8Encoding();
                    var hora = DateTime.Now;
                    var msg = string.Format(@"Enviando mensajes desde Ping! a las {0}",hora);
                        var msgBytes = encoding.GetBytes(msg);
                   
                    channel.BasicPublish("Servicios", "optional-routing-key", false, properties, msgBytes);
                    
                    channel.Close();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }


        }
       
    }
}
