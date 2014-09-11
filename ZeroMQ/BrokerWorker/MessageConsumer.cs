using System;
using System.Text;
using System.Threading;
using ZMQ;

namespace BrokerWorker
{
    class MessageConsumer
    {
        private readonly Encoding _defaultEncoding = Encoding.Unicode;

        public void Run(int i)
        {
            using (var context = new Context())
            {
                using (var socket = context.Socket(SocketType.REP))
                {
                    socket.Connect("tcp://localhost:5560");
                    Console.WriteLine("Socket connected!");

                    var doneCount = 0;
                    while (doneCount < 3)
                    {
                        Thread.Sleep(200);
                        var message = socket.Recv(_defaultEncoding);
                        Console.WriteLine("Received message {0} on consumer {1}", message, i);
                        socket.Send();

                        if (message == "Done")
                        {
                            doneCount++;
                        }
                    }
                }
            }
            Console.WriteLine("Consumer done!");
        }
    }
}