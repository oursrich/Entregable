using RabbitMQ.Client;
using ReceiveMessages;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServicesPing
{
    class WebServer
    {
        private TcpListener listener;
        private int port;
        private string home;
        private static int MAX_SIZE = 1000;
        public WebServer(int port, string path)
        {
            this.port = port;
            this.home = path;
             listener = new TcpListener(IPAddress.Any, port);
        }
        public void Start()
        {
            listener.Start();
            Console.WriteLine(string.Format("Local server started at localhost:{0}", port));

            Console.CancelKeyPress += delegate
            {
                Console.WriteLine("Stopping server");
                StopServer();
            };
        }
        public void Listen()
        {
            Request resques = new Request();
            string requestData = string.Empty;
            int i = 0;
            try
            {
                while (true)
                {



                    i++;

                    Byte[] result = new Byte[MAX_SIZE];
                    
                    TcpClient client = listener.AcceptTcpClient();
                    NetworkStream stream = client.GetStream();
                    int size = stream.Read(result, 0, result.Length);
                    requestData = System.Text.Encoding.ASCII.GetString(result, 0, size);
                    //Console.WriteLine("Received: {0}", requestData);
                    Request request = new Request();
                    
                    using (BlockingCollection<int> bc = new BlockingCollection<int>())
                    {

                        {

                            // Spin up a Task to consume the BlockingCollection
                            using (Task t2 = Task.Factory.StartNew(() =>
                            {
                                try
                                {
                                    // Consume consume the BlockingCollection

                                    if (!bc.IsCompleted)
                                    {
                                        var tasks = new List<Task<int>>();
                                        Task t = Task.Factory.StartNew(() => request = resques.GetRequest(requestData, stream));
                                        //bc.CompleteAdding();
                                        
                                    }
                                        
                                    
                                }
                                catch (InvalidOperationException)
                                {

                                    // An InvalidOperationException means that Take() was called on a completed collection
                                    Console.WriteLine("That's All!");
                                }
                            }))

                                Task.WaitAll(t2);
                        }
                        //bc.Dispose
                        Response receive = new Response();
                        var thread = new Thread(o => receive.ProcessResponse());
                        thread.Start();

                        //Task.WaitAll(t);

                        //resques.ProcessRequest(request, stream);

                        //Console.WriteLine(i);
                    }


                }
            }
            finally
            {
                listener.Stop();
            }
        }
        

        private void StopServer()
        {
            listener.Stop();
        }

        

     

        
    }

}
