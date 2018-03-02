using ReceiveMessages;
using SendMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesPing
{
    public class Response
    {

        public void ProcessResponse()
        {
            AsyncResponseMessage receive = new AsyncResponseMessage();
            receive.AsyncResponseMessages("Respuesta");
                
        }
    }
}
