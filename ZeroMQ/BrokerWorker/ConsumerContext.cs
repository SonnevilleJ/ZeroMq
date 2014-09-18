using System;
using System.Text;
using System.Threading.Tasks;
using ZMQ;

namespace BrokerWorker
{
    class ConsumerContext
    {
        public void Run()
        {
            var socketLock = new object();
            var messagesConsumed = 0;
            using (var context = new Context())
            using (var consumerSocket = context.Socket(SocketType.REP))
            using (var monitorSocket = context.Socket(SocketType.REQ))
            {
                monitorSocket.Connect("tcp://localhost:9999");
                Console.WriteLine("Monitor connected!");
                consumerSocket.Connect("tcp://localhost:5560");
                Console.WriteLine("Consumer connected!");

                Action consumer = () => new MessageConsumer().Run(consumerSocket, socketLock, ref messagesConsumed);
                Action monitor = () => new QueueMonitor().Monitor(monitorSocket, Encoding.UTF8, consumer, ref messagesConsumed);
                
                Task.Run(monitor).Wait();
            }
            Console.WriteLine("Consumer done!");
            Console.WriteLine("Consumed {0} messages!", messagesConsumed);
            Console.ReadLine();
        }
    }
}