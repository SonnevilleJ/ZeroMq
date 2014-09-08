using System;
using System.Text;
using ZMQ;

namespace BrokerClient
{
    static class ClientProgram
    {
        private static readonly Encoding DefaultEncoding = Encoding.Unicode;

        static void Main(string[] args)
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
                        socket.Send("Hello", DefaultEncoding);
                        Console.WriteLine("Message sent!");
                        var reply = socket.Recv(DefaultEncoding);
                        Console.WriteLine("Received reply: {0}", reply);
                    }
                }
            }
            Console.WriteLine("Client done!");
            Console.ReadLine();
        }
    }
}
