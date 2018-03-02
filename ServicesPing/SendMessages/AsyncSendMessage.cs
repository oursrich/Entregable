using RabbitMQ.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SendMessages
{
    public class AsyncSendMessage
    {
        public IConnection Connect()
        {


            try
            {
                var connectionFactory = new ConnectionFactory
                {
                    HostName = "localhost",
                    Port = 5672,
                    UserName = "guest",
                    Password = "guest",
                    Protocol = Protocols.AMQP_0_9_1,
                    RequestedFrameMax = UInt32.MaxValue,
                    RequestedHeartbeat = UInt16.MaxValue
                };
                var connection = connectionFactory.CreateConnection();
                return connection;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                IConnection connection = null;
                return connection;
            }


        }
        public void AsyncMessage(string queue)
        {
            SendMessage Message = new SendMessage();
            IConnection connection = Connect();
            if (connection.IsOpen)
            {

                var thread = new Thread(o => Message.Send(connection, queue));
                thread.Start();

                while (!thread.IsAlive)
                    Thread.Sleep(10);
            }
        }
    }
}

