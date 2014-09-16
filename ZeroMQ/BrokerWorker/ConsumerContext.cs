using System;
using ZMQ;

namespace BrokerWorker
{
    class ConsumerContext
    {
        public void Run()
        {
            using (var context = new Context())
            using (var socket = context.Socket(SocketType.REP))
            {
                socket.Connect("tcp://localhost:5560");
                Console.WriteLine("Socket connected!");

                new MessageConsumer().Run(socket);
            }
            Console.WriteLine("Consumer done!");
            Console.ReadLine();
        }
    }
}