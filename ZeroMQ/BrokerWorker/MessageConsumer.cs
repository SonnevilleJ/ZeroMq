using System;
using System.Text;
using System.Threading;
using ZMQ;

namespace BrokerWorker
{
    class MessageConsumer
    {
        private readonly Encoding _defaultEncoding = Encoding.Unicode;

        public void Run()
        {
            using (var context = new Context())
            {
                using (var socket = context.Socket(SocketType.REP))
                {
                    Thread.Sleep(5000);
                    socket.Connect("tcp://localhost:5560");
                    Console.WriteLine("Socket connected!");

                    while (true)
                    {
                        var message = socket.Recv(_defaultEncoding);
                        Console.WriteLine("Received message: {0}", message);
                        socket.Send();
                    }
                }
            }
        }
    }
}