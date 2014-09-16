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

        public void Run(string connectString)
        {
            using (var context = new Context())
            {
                using (var socket = context.Socket(SocketType.REP))
                {
                    socket.Connect(connectString);
                    Console.WriteLine("Socket connected!");

                    var doneCount = 0;
                    while (doneCount < 3)
                    {
                        Thread.Sleep(200);
                        var message = socket.Recv(_defaultEncoding);
                        Console.WriteLine("Received message {0} on consumer {1}", message, _id);
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