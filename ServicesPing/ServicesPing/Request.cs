using SendMessages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ServicesPing
{
    public class Request
    {
        public string Command;
        public string Path;
        public string Protocol;
        public string server;
        private static string NOTFOUND404 = "HTTP/1.1 404 Not Found";
        private static string OK200 = "HTTP/1.1 200 OK\r\n\r\n\r\n";
        private string home;
        public void ProcessRequest(Request request, NetworkStream stream)
        {
            
            if (request == null)
            {
                return;
            }
            AsyncSendMessage send = new AsyncSendMessage();
            var tasks = new List<Task<int>>();
            Task t = Task.Factory.StartNew(() =>send.AsyncMessage("Servicios"));
            Task.WaitAll(t);
            if (request.Path.Equals("/"))
                request.Path = "/home.html";
           

            GenerateResponse("Not found", stream, NOTFOUND404);
        }

       

        private void GenerateResponse(string content,
            NetworkStream stream,
            string responseHeader)
        {
            string response = "HTTP/1.1 200 OK\r\n\r\n\r\n";
            response = response + content;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(response);
            //stream.Write(msg, 0, msg.Length);
            return;
        }
        public Request GetRequest(string data, NetworkStream stream)
        {
            Request request = new Request();
            var list = data.Split(' ');
            if (list.Length < 3)
                return null;

            request.Command = list[0];
            request.Path = list[1];
            request.Protocol = list[2].Split('\n')[0];
            request.server = list[3].Split('\r')[0];
            
            ProcessRequest(request,stream);
            return request;
        }
        
    }
}
