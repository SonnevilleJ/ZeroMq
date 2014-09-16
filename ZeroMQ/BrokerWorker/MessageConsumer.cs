using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using ZMQ;

namespace BrokerWorker
{
    class MessageConsumer
    {
        private readonly Encoding _defaultEncoding = Encoding.Unicode;
        private readonly int _id;

        public MessageConsumer()
        {
            _id = Process.GetCurrentProcess().Id;
        }

        public void Run(Socket socket)
        {
            while (true)
            {
                Thread.Sleep(200);
                string message;
                lock (socket)
                {
                    message = socket.Recv(_defaultEncoding);
                    Console.WriteLine("Received message {0} on consumer {1}", message, _id);
                    socket.Send();
                }

                if (message == "Done") break;
            }
        }
    }
}