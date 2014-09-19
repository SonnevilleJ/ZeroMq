using System;
using System.Text;
using System.Threading.Tasks;
using Common;
using ZMQ;

namespace BrokerWorker
{
    class ConsumerContext
    {
        private readonly Encoding _encoding = Encoding.UTF8;

        public void Run()
        {
            var socketLock = new object();
            var messagesConsumed = 0;
            using (var context = new Context())
            using (var consumerSocket = context.Socket(SocketType.REP))
            using (var monitorSocket = context.Socket(SocketType.REQ))
            {
                var receiver = new MulticastReceiver(_encoding);
                var brokerIp = receiver.Receive("239.20.20.20", 8888);

                monitorSocket.Connect(string.Format("tcp://{0}:9999", brokerIp));
                Console.WriteLine("Monitor connected!");
                consumerSocket.Connect(string.Format("tcp://{0}:5560", brokerIp));
                Console.WriteLine("Consumer connected!");

                Action consumer = () => new MessageConsumer().Run(consumerSocket, socketLock, ref messagesConsumed);
                Action monitor = () => new QueueMonitor().Monitor(monitorSocket, _encoding, consumer, ref messagesConsumed);
                Action alerter = () => new EmailAlerter().Run(monitorSocket, _encoding, ref messagesConsumed);

                var tasks = new[]
                {
                    Task.Run(monitor),
                    Task.Run(alerter)
                };
                Task.WaitAll(tasks);
            }
            Console.WriteLine("Consumer done!");
            Console.WriteLine("Consumed {0} messages!", messagesConsumed);
            Console.ReadLine();
        }
    }
}