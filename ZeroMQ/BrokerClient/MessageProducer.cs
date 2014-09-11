using System;
using System.Text;
using ZMQ;

namespace BrokerClient
{
    class MessageProducer
    {
        private readonly Encoding _defaultEncoding = Encoding.Unicode;

        public void Run()
        {
            using (var context = new Context())
            {
                using (var socket = context.Socket(SocketType.REQ))
                {
                    socket.Connect("tcp://localhost:5559");
                    Console.WriteLine("Socket connected!");

                    const int requestsToSend = 10;
                    for (var i = 0; i < requestsToSend; i++)
                    {
                        Console.WriteLine("Sending message {0}...", i);
                        socket.Send("Hello", _defaultEncoding);
                        socket.Recv();
                    }
                }
            }
            Console.WriteLine("Client done!");
            Console.ReadLine();
        }
    }
}