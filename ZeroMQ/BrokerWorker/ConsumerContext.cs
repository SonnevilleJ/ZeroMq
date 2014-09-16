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
            using (var context = new Context())
            using (var consumerSocket = context.Socket(SocketType.REP))
            using (var monitorSocket = context.Socket(SocketType.REQ))
            {
                monitorSocket.Connect("tcp://localhost:9999");
                Console.WriteLine("Monitor connected!");
                consumerSocket.Connect("tcp://localhost:5560");
                Console.WriteLine("Consumer connected!");

                Action consumer = () => new MessageConsumer().Run(consumerSocket, socketLock);
                Action monitor = () => new QueueMonitor().Monitor(monitorSocket, Encoding.Unicode, consumer);

                var tasks = new[]
                {
                    Task.Run(monitor)
                };
                Task.WaitAll(tasks);
            }
            Console.WriteLine("Consumer done!");
            Console.ReadLine();
        }
    }
}